using System.ComponentModel.DataAnnotations;

namespace BL.DTO
{
    /// <summary>
    /// Wrapper for user login details
    /// </summary>
    public class UserLoginDTO
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}