using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTO;

namespace BL.Filters
{
    public class EventFilter
    {
        public string Artist { get; set; }

        public int? ArtistId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy")]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy")]
        public DateTime End { get; set; }

        public string Place { get; set; }

        public string Name { get; set; }

        public Guid? CreatorId { get; set; }
    }
}
