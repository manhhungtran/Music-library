using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BL.DTO;
using BL.DTO.Event;
using BL.Facades;
using BL.Filters;
using BL.Utilities.AccountPolicy;
using PL.Models;
using X.PagedList;
using ActionResult = System.Web.Mvc.ActionResult;
using Controller = System.Web.Mvc.Controller;

namespace PL.Controllers
{
    [Authorize(Roles = Claims.Vips)]
    public class EventController : Controller
    {
        private readonly string filterSessionKey = "filterevent";

        public PremiumFacade PremiumFacade { get; set; }
        public LibraryFacade LibraryFacade { get; set; }

        public UserFacade UserFacade { get; set; }

        public ActionResult Index(int page = 1)
        {
            EventFilter filter = Session[filterSessionKey] as EventFilter ?? new EventFilter();

            EventListQueryResultDTO result = PremiumFacade.GetEvents(filter, page);

            EventListViewModel model = InitNewEvent(result, filter);

            return View("List", model);
        }


        [HttpPost]
        public ActionResult Index(EventListViewModel model)
        {
            Session[filterSessionKey] = model.Filter;

            var result = PremiumFacade.GetEvents(model.Filter);

            var newModel = InitNewEvent(result, model.Filter);

            return View("List", newModel);
        }

        public ActionResult ClearFilter()
        {
            Session[filterSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Details(int id)
        {
            var model = PremiumFacade.GetEvent(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                return View("Details", model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var model = PremiumFacade.GetEvent(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                PremiumFacade.DeleteEvent(id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            InitBasics();
            return View("Create", new EventDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventDTO model)
        {
            model.Creator = UserFacade.GetGuidByEmail(User.Identity.Name);
            if (model.ArtistPId != null) { model.Artist = LibraryFacade.GetArtist(model.ArtistPId.Value);}
            if (ModelState.IsValid)
            {
                try
                {
                    PremiumFacade.CreateEvent(model);
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
            var filter = new ArtistFilter
            {
                CreatorId = UserFacade.GetGuidByEmail(User.Identity.Name)
            };
            ViewBag.Artists = LibraryFacade.GetArtistBasicsByFilter(filter);
        }

        public ActionResult Edit(int id)
        {
            var model = PremiumFacade.GetEvent(id);
            if (model.Creator == UserFacade.GetGuidByEmail(User.Identity.Name))
            {
                InitBasics();
                return View("Edit", model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PremiumFacade.EditEvent(model);
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


        private EventListViewModel InitNewEvent(EventListQueryResultDTO result, EventFilter filter)
        {
            return new EventListViewModel
            {
                Events = new StaticPagedList<EventDTO>(result.ResultsPage, result.RequestedPage, PremiumFacade.PageSize, result.TotalResultCount),
                Filter = filter
            };
        }
    }
}
