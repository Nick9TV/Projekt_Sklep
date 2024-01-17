using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Projekt_Sklep.Models.Requests
{
    public class UserRegisterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required, RegularExpression(@"^\d{9}$", ErrorMessage = "Podano zły numer telfonu.")]
        public string Phone { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
