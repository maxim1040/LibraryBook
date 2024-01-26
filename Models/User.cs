using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Security.Permissions;

namespace LibraryBook.Models
{
    public class User
    {
        [Key]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is verplicht")]
        [StringLength(50, ErrorMessage = "Username kan niet langer zijn dan 50 karakters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Passwoord is verplicht")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Passwoord kan niet langer zijn dan 100 karakters")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Email adress is niet geldig")]
        public string Email { get; set; }

        public ICollection<Loan> Loans { get; set; } = default!;    
    }
}
