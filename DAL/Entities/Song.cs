using System;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Song : IEntity<int>
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public string Genres { get; set; }

        [Required]
        public TimeSpan Length { get; set; }

        public virtual Artist Artist { get; set; }

        public virtual Album Album { get; set; }

        public int ID { get; set; }

        [Required]
        public Guid Creator { get; set; }

        public int AlbumPId { get; set; }

        public int ArtistPId { get; set; }

        protected bool Equals(Song other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) 
                && Length.Equals(other.Length) 
                && Equals(Artist, other.Artist);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Song) obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
