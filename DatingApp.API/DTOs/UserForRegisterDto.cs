using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must between 5 to 20 characters")]
        public string Password { get; set; }
    }
}