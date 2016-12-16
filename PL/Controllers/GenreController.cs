using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Genre;
using BL.Facades;
using BL.Filters;
using BL.Utilities.AccountPolicy;
using PL.Models;
using X.PagedList;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.AuthenticatedUsers)]
    public class GenreController : Controller
    {
        private readonly string filterSessionKey = "filtergenre";

        public LibraryFacade LibraryFacade { get; set; }

        public UserFacade UserFacade { get; set; }

        public ActionResult Index(int page = 1)
        {
            GenreFilter filter = Session[filterSessionKey] as GenreFilter ?? new GenreFilter();

            GenreListQueryResultDTO result = LibraryFacade.GetGenres(filter, page);

            GenreListViewModel model = InitNewModel(result, filter);

            return View("List", model);
        }


        [HttpPost]
        public ActionResult Index(GenreListViewModel model)
        {
            Session[filterSessionKey] = model.Filter;

            var result = LibraryFacade.GetGenres(model.Filter);

            var newModel = InitNewModel(result, model.Filter);

            return View("List", newModel);
        }

        public ActionResult ClearFilter()
        {
            Session[filterSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            LibraryFacade.DeleteGenre(id);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View("Create", new GenreDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GenreDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LibraryFacade.CreateGenre(model);
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
            var model = LibraryFacade.GetGenre(id);
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GenreDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LibraryFacade.EditGenre(model);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return RedirectToAction("Edit", model);
        }


        private GenreListViewModel InitNewModel(GenreListQueryResultDTO result, GenreFilter filter)
        {
            return new GenreListViewModel
            {
                Genres = new StaticPagedList<GenreDTO>(result.ResultsPage, result.RequestedPage, LibraryFacade.PageSize, result.TotalResultCount),
                Filter = filter
            };
        }
    }
}