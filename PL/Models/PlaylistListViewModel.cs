using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BL.DTO;
using BL.Filters;
using BL.Utilities.SortEnum;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace PL.Models
{
    public class PlaylistListViewModel
    {
        public IPagedList<PlaylistDTO> Playlists { get; set; }

        public PlaylistFilter Filter { get; set; }
    }
}