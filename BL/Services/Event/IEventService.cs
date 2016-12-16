using System.Collections.Generic;
using BL.DTO;
using BL.DTO.Event;
using BL.Filters;
using SelectListItem = System.Web.Mvc.SelectListItem;

namespace BL.Services
{
    public interface IEventService
    {
        /// <summary>
        /// Creates new event
        /// </summary>
        /// <param name="eventDto">Event that should be created</param>
        void CreateEvent(EventDTO eventDto);

        /// <summary>
        /// Updates event according to <see cref="EventDTO.ID"/>
        /// </summary>
        /// <param name="eventDto"></param>
        void EditEvent(EventDTO eventDto);

        /// <summary>
        /// Deletes event according to <param name="eventId"/>.
        /// </summary>
        void DeleteEvent(int eventId);

        /// <summary>
        /// Returns specific event according to <param name="eventId"/>
        /// </summary>
        EventDTO GetEvent(int eventId);

        /// <summary>
        /// Returns all events
        /// </summary>
        IEnumerable<EventDTO> GetAllEvents();

        /// <summary>
        /// Get events according to page and filter
        /// </summary>
        /// <param name="requestedPage">page to display</param>
        /// <param name="filter">event filter</param>
        /// <returns></returns>
        EventListQueryResultDTO ListEvents(int requestedPage, EventFilter filter = null);

        /// <summary>
        /// Returns all events by filter
        /// </summary>
        IEnumerable<EventDTO> GetEventsByFilter(EventFilter filter);
    }
}
