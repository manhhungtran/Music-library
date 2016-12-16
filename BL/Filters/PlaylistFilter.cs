using System;
using System.Collections.Generic;
using DAL.Entities;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BL.Filters
{
    public class PlaylistFilter
    {
        public string Name { get; set; }

        public Guid? CreatorId { get; set; }
    }
}
