using BL.DTO;
using BL.Filters;
using X.PagedList;

namespace PL.Models
{
    public class AlbumListViewModel
    {
        public IPagedList<AlbumDTO> Albums { get; set; }

        public AlbumFilter Filter { get; set; }
    }
}