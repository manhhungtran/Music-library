using System;
using System.Collections.Generic;

namespace BL.Filters
{
    public class SongFilter
    {
        public string Name { get; set; }

        public string ArtistName { get; set; }

        public bool? GenresAll { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public int ArtistId { get; set; }

        public int? AlbumId { get; set; }

        public Guid? CreatorId { get; set; }
    }
}
