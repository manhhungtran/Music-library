using System;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Event : IEntity<int>
    {
        [Required]
        public string Name { get; set; }

        public virtual Artist Artist { get; set; }

        public int? ArtistPId { get; set; }

        [Required]
        public DateTime? Start { get; set; }

        [Required]
        public DateTime? End { get; set; }

        [Required]
        public string Place { get; set; }

        [MaxLength(65536)]
        public string Description { get; set; }

        [Required]
        public Guid Creator { get; set; }

        public int ID { get; set; }

        protected bool Equals(Event other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) 
                && Equals(Artist, other.Artist) && Start.Equals(other.Start) 
                && string.Equals(Place, other.Place, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Event) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode*397) ^ (Artist?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ Start.GetHashCode();
                hashCode = (hashCode*397) ^ (Place != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Place) : 0);
                return hashCode;
            }
        }
    }
}
