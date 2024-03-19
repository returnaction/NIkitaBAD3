using Microsoft.AspNetCore.Identity;

namespace NIkitaBAD3.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Casino { get; set; }
        public string? Position { get; set; }

        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

    }
}
