using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Song;
using BL.Facades;
using BL.Filters;
using BL.Utilities.AccountPolicy;
using PL.Models;
using X.PagedList;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.AuthenticatedUsers)]
    public class SongController : Controller
    {
        private readonly string filterSessionKey = "filtersong";

        public LibraryFacade LibraryFacade { get; set; }

        public UserFacade UserFacade { get; set; }

        public ActionResult Index(int page = 1)
        {
            SongFilter filter = Session[filterSessionKey] as SongFilter ?? new SongFilter();

            SongListQueryResultDTO result = LibraryFacade.GetSongs(filter, page);

            SongListViewModel model = InitNewSong(result, filter);

            return View("List", model);
        }


        [HttpPost]
        public ActionResult Index(SongListViewModel model)
        {
            Session[filterSessionKey] = model.Filter;

            var result = LibraryFacade.GetSongs(model.Filter);

            var newModel = InitNewSong(result, model.Filter);

            return View("List", newModel);
        }

        public ActionResult ClearFilter()
        {
            Session[filterSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Details(int id)
        {
            var model = LibraryFacade.GetSong(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                model.Genres = LibraryFacade.GetGenreNames(model.Genres);
                return View("Details", model);
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult Delete(int id)
        {
            var model = LibraryFacade.GetSong(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                LibraryFacade.DeleteSong(id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            GetBasics();
            return View("Create", new SongDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SongDTO model)
        {
            model.Creator = UserFacade.GetGuidByEmail(User.Identity.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    model.Genres = LibraryFacade.FormGenres(model.Genres);
                    LibraryFacade.CreateSong(model);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            GetBasics();
            return View("Create", model);
        }

        private void GetBasics()
        {
            AlbumFilter filter = new AlbumFilter
            {
                CreatorId = UserFacade.GetGuidByEmail(User.Identity.Name)
            };
            ArtistFilter filterA = new ArtistFilter
            {
                CreatorId = UserFacade.GetGuidByEmail(User.Identity.Name)
            };

            ViewBag.Albums = LibraryFacade.GetAlbumBasicsByFilter(filter);
            ViewBag.Artists = LibraryFacade.GetArtistBasicsByFilter(filterA);
        }

        public ActionResult Edit(int id)
        {
            var model = LibraryFacade.GetSong(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                GetBasics();
                model.Genres = LibraryFacade.GetGenreNames(model.Genres);
                return View("Edit", model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SongDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Genres = LibraryFacade.FormGenres(model.Genres);
                    LibraryFacade.EditSong(model);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return RedirectToAction("Edit", model);
        }


        private SongListViewModel InitNewSong(SongListQueryResultDTO result, SongFilter filter)
        {
            return new SongListViewModel()
            {
                Songs = 
                    new StaticPagedList<SongDTO>(result.ResultsPage, result.RequestedPage, LibraryFacade.PageSize, result.TotalResultCount),
                Filter = filter
            };
        }
    }
}
