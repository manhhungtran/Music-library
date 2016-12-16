using System;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace DAL.Entities
{
    public class Playlist : IEntity<int>
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public string Songs { get; set; }

        [Required]
        public Guid Creator { get; set; }

        public string Genres { get; set; }

        public int ID { get; set; }


    }
}
