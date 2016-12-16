using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Album;
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
    public class AlbumController : Controller
    {
        private readonly string filterSessionKey = "filteralbum";

        public LibraryFacade LibraryFacade { get; set; }

        public UserFacade UserFacade { get; set; }

        public ActionResult Index(int page = 1)
        {
            AlbumFilter filter = Session[filterSessionKey] as AlbumFilter ?? new AlbumFilter();

            AlbumListQueryResultDTO result = LibraryFacade.GetAlbums(filter, page);

            AlbumListViewModel model = InitNewAlbum(result, filter);

            return View("List", model);
        }


        [HttpPost]
        public ActionResult Index(AlbumListViewModel model)
        {
            Session[filterSessionKey] = model.Filter;

            var result = LibraryFacade.GetAlbums(model.Filter);

            var newModel = InitNewAlbum(result, model.Filter);

            return View("List", newModel);
        }

        public ActionResult ClearFilter()
        {
            Session[filterSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Details(int id)
        {
            var model = LibraryFacade.GetAlbum(id);
            string result = "";
            foreach (var song in LibraryFacade.GetAllSongs())
            {
                if (song.AlbumPId == id)
                {
                    result += song.Name + ", ";
                }
            }
            if (result.Length > 2)
            {
                result = result.Substring(0, result.Length - 2);
                ViewBag.Songs = result;
            }
            else
            {
                ViewBag.Songs = "";
            }
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                return View("Details", model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var model = LibraryFacade.GetAlbum(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                LibraryFacade.DeleteAlbum(id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            InitBasics();
            return View("Create", new AlbumDTO());
        }

        private void InitBasics()
        {
            var filter = new ArtistFilter
            {
                CreatorId = UserFacade.GetGuidByEmail(User.Identity.Name)
            };
            ViewBag.Artists = LibraryFacade.GetArtistBasicsByFilter(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlbumDTO model)
        {
            model.Creator = UserFacade.GetGuidByEmail(User.Identity.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    LibraryFacade.CreateAlbum(model);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            InitBasics();
            return View("Create", model);
        }

        public ActionResult Edit(int id)
        {
            var model = LibraryFacade.GetAlbum(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                InitBasics();
                return View("Edit", model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlbumDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LibraryFacade.EditAlbum(model);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            InitBasics();
            return RedirectToAction("Edit", model);
        }

        private AlbumListViewModel InitNewAlbum(AlbumListQueryResultDTO result, AlbumFilter filter)
        {
            return new AlbumListViewModel
            {
                Albums = new StaticPagedList<AlbumDTO>(result.ResultsPage, result.RequestedPage, LibraryFacade.PageSize, result.TotalResultCount),
                Filter = filter
            };
        }
    }
}