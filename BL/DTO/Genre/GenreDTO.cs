using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    public class GenreDTO
    { 
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public int ID { get; set; }

        protected bool Equals(GenreDTO other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GenreDTO) obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
