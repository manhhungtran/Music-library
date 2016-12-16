using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    public class PlaylistDTO
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public List<string> Songs { get; set; }

        [Required]
        public Guid Creator { get; set; }

        public int ID { get; set; }

        protected bool Equals(PlaylistDTO other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && Creator.Equals(other.Creator) && ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlaylistDTO) obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
