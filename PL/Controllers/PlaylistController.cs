using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Playlist;
using BL.Facades;
using BL.Filters;
using BL.Utilities.AccountPolicy;
using Microsoft.Ajax.Utilities;
using PL.Models;
using X.PagedList;
using ActionResult = System.Web.Mvc.ActionResult;
using Controller = System.Web.Mvc.Controller;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.Vips)]
    public class PlaylistController : Controller
    {
        private readonly string filterSessionKey = "filterartist";

        public LibraryFacade LibraryFacade { get; set; }
        public PremiumFacade PremiumFacade { get; set; }

        public UserFacade UserFacade { get; set; }

        public ActionResult Index(int page = 1)
        {
            PlaylistFilter filter = Session[filterSessionKey] as PlaylistFilter ?? new PlaylistFilter();

            PlaylistListQueryResultDTO result = LibraryFacade.GetPlaylists(filter, page);

            PlaylistListViewModel model = InitNewPlaylist(result, filter);

            return View("List", model);
        }

        [HttpPost]
        public ActionResult Index(PlaylistListViewModel model)
        {
            Session[filterSessionKey] = model.Filter;

            var result = LibraryFacade.GetPlaylists(model.Filter);

            var newModel = InitNewPlaylist(result, model.Filter);

            return View("List", newModel);
        }

        
        public ActionResult Rec(int id)
        {
            var result = new PlaylistListModel();
            result.Playlists = PremiumFacade.GetRecommendedPlaylists(LibraryFacade.GetPlaylist(id));
            return View("Rec", result);
        }

        public ActionResult ClearFilter()
        {
            Session[filterSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Details(int id)
        {
            var model = LibraryFacade.GetPlaylist(id);
            List<string> genres = new List<string>();
            foreach (var s in model.Songs)
            {
                if (s.IsNullOrWhiteSpace()) continue;
                foreach (string g in LibraryFacade.GetSong(Convert.ToInt32(s)).Genres)
                {
                    if (!genres.Contains(g))
                    {
                        genres.Add(g);
                    }
                }
            }
            var res = LibraryFacade.GetGenreNames(genres);
            string result = "";
            foreach (var g in res)
            {
                result = g + ", ";
            }
            if (result.Length > 2)
            {
                result = result.Substring(0, result.Length - 2);
                ViewBag.Genres = result;
            }
            else
            {
                ViewBag.Genres = "";
            }
            InitBasics();
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                return View("Details", model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var model = LibraryFacade.GetPlaylist(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                LibraryFacade.DeletePlaylist(id);
            }
            InitBasics();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View("Create", new PlaylistDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlaylistDTO model)
        {
            model.Creator = UserFacade.GetGuidByEmail(User.Identity.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    LibraryFacade.CreatePlaylist(model);
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

        private void InitBasics()
        {
            var filter = new SongFilter
            {
                CreatorId = UserFacade.GetGuidByEmail(User.Identity.Name)
            };
            ViewBag.Songs = LibraryFacade.GetSongBasicsByFilter(filter);
        }

        public ActionResult Edit(int id)
        {
            var model = LibraryFacade.GetPlaylist(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                return View("Edit", model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlaylistDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LibraryFacade.EditPlaylist(model);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return RedirectToAction("Edit", model);
        }


        private PlaylistListViewModel InitNewPlaylist(PlaylistListQueryResultDTO result, PlaylistFilter filter)
        {
            return new PlaylistListViewModel
            {
                Playlists = new StaticPagedList<PlaylistDTO>(result.ResultsPage, result.RequestedPage, LibraryFacade.PageSize, result.TotalResultCount),
                Filter = filter
            };
        }

        public ActionResult AddSong(int id)
        {
            ViewBag.Playlists = PremiumFacade.GetSelectedItems(UserFacade.GetGuidByEmail(User.Identity.Name));
            return View("Pick", new AddSongToPlaylistModel { IdSong = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSong(AddSongToPlaylistModel model)
        {
            if (ModelState.IsValid)
            {
                PremiumFacade.AddSongToPlaylist(model.Playlist, model.IdSong);
                return RedirectToAction("Index", "Song");
            }
            
            return View("Pick", model);
        }

        public ActionResult RemoveSong(int song, int playlist)
        {
            PremiumFacade.RemoveSongFromPlaylist(playlist, song);
            return RedirectToAction("Details", new {id = playlist});
        }

    }
}
