using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class Artist : IEntity<int>
    {
        public int ID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(256)]
        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public string Description { get; set; }

        public string Images { get; set; }

        [Required]
        public Guid Creator { get; set; }

        protected bool Equals(Artist other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Artist) obj);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}
