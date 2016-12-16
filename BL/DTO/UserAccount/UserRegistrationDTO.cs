using System;
using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    /// <summary>
    /// Wrapper for user registration details
    /// </summary>
    public class UserRegistrationDTO
    {
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(128)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        public string MobilePhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}, {this.Email}";
        }
    }
}
