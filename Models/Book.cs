using LibraryBook.Areas.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Book.cs
// Book.cs
namespace LibraryBook.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, ErrorMessage = "Author cannot be longer than 100 characters.")]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(13, ErrorMessage = "ISBN should be 13 characters long.")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Display(Name = "Loan Status")]
        public bool IsLoaned { get; set; }

        [ForeignKey("Loaner")]
        [Display(Name = "Geleend door")]
        public string? LoanerUserName { get; set; }
        public LibraryUser? Loaner { get; set; }

        // Navigatie-eigenschappen
        public List<Loan> Loans { get; set; }
    }
}

