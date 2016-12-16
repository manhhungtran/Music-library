using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    public class SongDTO
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public List<string> Genres { get; set; }

        [Required]
        [DataType(DataType.Duration)]
        public TimeSpan Length { get; set; }

        public virtual ArtistDTO Artist { get; set; }

        public virtual AlbumDTO Album { get; set; }

        public int AlbumPId { get; set; }

        public int ArtistPId { get; set; }

        public int ID { get; set; }

        [Required]
        public Guid Creator { get; set; }

        protected bool Equals(SongDTO other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && Artist == other.Artist && ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SongDTO) obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
