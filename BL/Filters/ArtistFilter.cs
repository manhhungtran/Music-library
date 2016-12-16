using System;
using System.Collections.Generic;
using DAL.Entities;

namespace BL.Filters
{
    public class ArtistFilter
    {
        public string Name { get; set; }

        public Guid? CreatorId { get; set; }
    }
}
