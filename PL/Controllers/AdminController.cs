using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.UserAccount;
using BL.Facades;
using BL.Filters;
using BL.Utilities.AccountPolicy;
using PL.Models;
using X.PagedList;
using ActionResult = System.Web.Mvc.ActionResult;
using Controller = System.Web.Mvc.Controller;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.Admin)]
    public class AdminController : Controller
    {
        private readonly string filterSessionKey = "filterforasflasjgfkjh";

        public LibraryFacade LibraryFacade { get; set; }

        public UserFacade UserFacade { get; set; }

        [Authorize(Roles = Claims.AuthenticatedUsers)]
        public ActionResult Settings()
        {
            UserPasswordDTO model = new UserPasswordDTO()
            {
                Username = User.Identity.Name
            };
            return View("ChangePassword", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(UserPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserFacade.ChangePassword(model);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View("ChangePassword");
        }

        public ActionResult Promote(int page = 1)
        {
            var filter = Session[filterSessionKey] as VipCodeFilter ?? new VipCodeFilter();

            VipCodesListQueryResultsDTO result = UserFacade.ListCodes(filter, page);

            VipCodeListViewModel model = InitNewModel(result, filter);

            return View("List", model);
        }


        [HttpPost]
        public ActionResult Promote(VipCodeListViewModel model)
        {
            
            Session[filterSessionKey] = model.Filter;

            var result = UserFacade.ListCodes(model.Filter);

            VipCodeListViewModel newModel = InitNewModel(result, model.Filter);

            return View("List", newModel);
        }


        private VipCodeListViewModel InitNewModel(VipCodesListQueryResultsDTO result, VipCodeFilter filter)
        {
            return new VipCodeListViewModel
            {
                Filter = filter,
                Codes = new StaticPagedList<VipCodesDTO>(result.ResultsPage, result.RequestedPage, LibraryFacade.PageSize, result.TotalResultCount),
            };
        }

        public ActionResult ClearFilter()
        {
            Session[filterSessionKey] = null;
            return RedirectToAction(nameof(Promote));
        }

        public ActionResult Delete(int id)
        {
            UserFacade.RemoveVip(id);
            return RedirectToAction(nameof(Promote));
        }

        public ActionResult Upgrade(Guid id)
        {
            var vipCode = UserFacade.GetCodeById(id);
            UserFacade.PromoteUser(id, false, true);
            UserFacade.RemoveVip(vipCode.ID);
            return RedirectToAction(nameof(Promote));
        }
    }
}