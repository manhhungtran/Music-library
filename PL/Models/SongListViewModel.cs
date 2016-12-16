using BL.DTO;
using BL.Filters;
using X.PagedList;

namespace PL.Models
{
    public class SongListViewModel
    {
        public IPagedList<SongDTO> Songs { get; set; }

        public SongFilter Filter { get; set; }
    }
}