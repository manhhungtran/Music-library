using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BL.Filters
{
    public class AlbumFilter
    {
        public string Name { get; set; }

        public string Artist { get; set; }

        public int? ArtistId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy")]
        public DateTime? PublishDateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy")]
        public DateTime? PublishDateTo { get; set; }

        public Guid? CreatorId { get; set; }
    }
}
