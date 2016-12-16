using BL.DTO;
using BL.Filters;
using X.PagedList;

namespace PL.Models
{
    public class EventListViewModel
    {
        public IPagedList<EventDTO> Events { get; set; }

        public EventFilter Filter { get; set; }
    }
}