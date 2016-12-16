using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Artist;
using BL.Facades;
using BL.Filters;
using BL.Utilities.AccountPolicy;
using PL.Models;
using X.PagedList;
using ActionResult = System.Web.Mvc.ActionResult;
using Controller = System.Web.Mvc.Controller;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.AuthenticatedUsers)]
    public class ArtistController : Controller
    {
        private readonly string filterSessionKey = "filterartist";

        public LibraryFacade LibraryFacade { get; set; }

        public UserFacade UserFacade { get; set; }

        public ActionResult Index(int page = 1)
        {
            ArtistFilter filter = Session[filterSessionKey] as ArtistFilter ?? new ArtistFilter();

            ArtistListQueryResultDTO result = LibraryFacade.GetArtists(filter, page);

            ArtistListViewModel model = InitNewArtist(result, filter);

            return View("List", model);
        }
        
        [HttpPost]
        public ActionResult Index(ArtistListViewModel model)
        {
            Session[filterSessionKey] = model.Filter;

            var result = LibraryFacade.GetArtists(model.Filter);

            var newModel = InitNewArtist(result, model.Filter);

            return View("List", newModel);
        }

        public ActionResult ClearFilter()
        {
            Session[filterSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Details(int id)
        {
            var model = LibraryFacade.GetArtist(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                return View("Details", model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var model = LibraryFacade.GetArtist(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                LibraryFacade.DeleteArtist(id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View("Create", new ArtistDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArtistDTO model)
        {
            model.Creator = UserFacade.GetGuidByEmail(User.Identity.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    LibraryFacade.CreateArtist(model);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View("Create", model);
        }

        public ActionResult Edit(int id)
        {
            var model = LibraryFacade.GetArtist(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                return View("Edit", model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArtistDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LibraryFacade.EditArtist(model);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return RedirectToAction("Edit", model);
        }


        private ArtistListViewModel InitNewArtist(ArtistListQueryResultDTO result, ArtistFilter filter)
        {
            return new ArtistListViewModel
            {
                Artists = new StaticPagedList<ArtistDTO>(result.ResultsPage, result.RequestedPage, LibraryFacade.PageSize, result.TotalResultCount),
                Filter = filter
            };
        }
    }
}
