using System;
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
        [Required]
        public string Gender {get;set;}
        [Required]
        public string KnownAs {get;set;}
        [Required]
        public DateTime DateObBirth {get;set;}
        [Required]
        public string City {get;set;}
        [Required]
        public string Country {get;set;}
        public DateTime Created {get;set;}
        public DateTime LastActive {get;set;}

        public UserForRegisterDto(){
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}