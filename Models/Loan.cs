using LibraryBook.Areas.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace LibraryBook.Models
{
    public class Loan
    {
        [Key]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        [Display(Name = "Datum begin lening")]
        public DateTime LoanDate { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        [Display(Name = "Datum einde lening")]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        public DateTime Deleted { get; set; }

        [ForeignKey("Book")]
        public int? BookId { get; set; }
        public Book? Book { get; set; }

        [ForeignKey("Loaner")]
        public string? LoanerId { get; set; }
        public LibraryUser? Loaner { get; set; }

        [Display(Name = "Titel van de boek")]
        public string? BookTitle => Book?.Title;

        [Display(Name = "Gebruikersnaam")]
        public string? LoanerUsername => Loaner?.UserName;
    }
}
