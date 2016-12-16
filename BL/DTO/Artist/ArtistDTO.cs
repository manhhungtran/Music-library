using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    public class ArtistDTO
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Profile picture")]
        public string ProfilePicture { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(65536)]
        public string Description { get; set; }

        public string Images { get; set; }

        [Required]
        public Guid Creator { get; set; }

        protected bool Equals(ArtistDTO other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ArtistDTO) obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
