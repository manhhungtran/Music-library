using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Album : IEntity<int>
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public virtual Artist Artist { get; set; }
        
        public int? ArtistPId { get; set; }

        [Required]
        public DateTime? PublishDate { get; set; }

        [MaxLength(65536)]

        public string Description { get; set; }

        public string Images { get; set; }

        [Required]
        public Guid Creator { get; set; }

        protected bool Equals(Album other)
        {
            return ID == other.ID && string.Equals(Name, other.Name) && Equals(Artist, other.Artist);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Album) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ID;
                hashCode = (hashCode*397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (Artist?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
