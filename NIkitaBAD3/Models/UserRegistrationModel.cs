using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace NIkitaBAD3.Models
{
    public class UserRegistrationModel
    {
        [MaxLength(25, ErrorMessage = "The max length is 25 characters")]
        public string? FirstName { get; set; }

        [MaxLength(25, ErrorMessage = "The max length is 25 characters")]
        public string? LastName { get; set; }


        [MaxLength(25, ErrorMessage = "The max length is 25 characters")]
        public string? Casino { get; set; }

        [MaxLength(25, ErrorMessage = "The max length is 25 characters")]
        public string? Position { get; set; }



        [MaxLength(100, ErrorMessage = "The max length is 100 characters")]
        public string? StreetAddress { get; set; }

        [MaxLength(25, ErrorMessage = "The max length is 25 characters")]
        public string? City { get; set; }

        [MaxLength(25, ErrorMessage = "The max length is 25 characters")]
        public string? State { get; set; }

        [MaxLength(25, ErrorMessage = "The max length is 25 characters")]
        public string? Country { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public int Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;


    }
}
