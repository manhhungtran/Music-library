using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTO.UserAccount
{
    public class VipCodesDTO
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
