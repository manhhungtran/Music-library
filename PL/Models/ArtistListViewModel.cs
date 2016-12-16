using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.DTO;
using BL.Filters;
using BL.Utilities.SortEnum;
using X.PagedList;

namespace PL.Models
{
    public class ArtistListViewModel
    {
        public IPagedList<ArtistDTO> Artists { get; set; }

        public ArtistFilter Filter { get; set; }

        public SelectList AllSortCriteria => new SelectList(Enum.GetNames(typeof(ArtistSortCriteria)).ToList());
    }
}