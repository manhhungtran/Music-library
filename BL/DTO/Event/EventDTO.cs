using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    public class EventDTO
    {
        [Required]
        public string Name { get; set; }

        public virtual ArtistDTO Artist { get; set; }

        public int? ArtistPId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? End { get; set; }

        [Required]
        public string Place { get; set; }

        [MaxLength(65536)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int ID { get; set; }

        [Required]
        public Guid Creator { get; set; }

        protected bool Equals(EventDTO other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && Artist == other.Artist && Start.Equals(other.Start) && string.Equals(Place, other.Place, StringComparison.InvariantCultureIgnoreCase) && ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EventDTO) obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
