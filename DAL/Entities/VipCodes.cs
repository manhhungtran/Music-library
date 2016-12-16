using System;
using System.ComponentModel.DataAnnotations;
using Riganti.Utils.Infrastructure.Core;

namespace DAL.Entities
{
    public class VipCodes : IEntity<int>
    {
        public int ID { get; set; }

        [Required]
        public Guid User { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
