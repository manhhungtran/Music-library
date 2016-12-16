using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    public class AlbumDTO
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ArtistDTO Artist { get; set; }

        [Required]
        public int? ArtistPId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PublishDate { get; set; }

        [MaxLength(65536)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Images { get; set; }

        [Required]
        public Guid Creator { get; set; }

        protected bool Equals(AlbumDTO other)
        {
            return ID == other.ID 
                &&  Artist.ID == other.Artist.ID
                && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AlbumDTO) obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
