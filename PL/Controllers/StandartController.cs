using System;
using System.Web.Mvc;
using BL.DTO.UserAccount;
using BL.Facades;
using BL.Utilities.AccountPolicy;
using ActionResult = System.Web.Mvc.ActionResult;
using Controller = System.Web.Mvc.Controller;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.Standard)]
    public class StandartController : Controller
    {
        public UserFacade UserFacade { get; set; }
        public LibraryFacade LibraryFacade { get; set; }

        public ActionResult Propose()
        {
            return View("Index");
        }

        public ActionResult BegForPromotion()
        {
            ViewBag.Message = "Done now you just have to wait...";

            UserFacade.CreateVip(UserFacade.GetGuidByEmail(User.Identity.Name), User.Identity.Name);
            return View("Index");
        }

        public ActionResult Claim()
        {
            Guid userGuid = UserFacade.GetGuidByEmail(User.Identity.Name);
            var vipCode = UserFacade.GetCodeById(userGuid);

            UserFacade.PromoteUser(userGuid, false, true);
            UserFacade.RemoveVip(vipCode.ID);
            return View("Claim", new VipCodesDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Claim(VipCodesDTO model)
        {
            model.User = UserFacade.GetGuidByEmail(User.Identity.Name);
            model.UserName = User.Identity.Name;

            if (ModelState.IsValid)
            {
                var vipCode = UserFacade.GetCodeById(model.User);
                if (vipCode.Code == model.Code)
                {
                    UserFacade.PromoteUser(model.User, false, true);
                    UserFacade.RemoveVip(vipCode.ID);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("Wrong code...", "");
            return View("Claim", model);
        }
    }
}