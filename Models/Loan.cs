using LibraryBook.Areas.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryBook.Models
{
    public class Loan
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        public DateTime Deleted { get; set; }

        [ForeignKey("Book")]
        public int? BookId { get; set; }
        public Book? Books { get; set; }

        [ForeignKey("Loaner")]  // Corrected from "LibraryUser"
        public string? LoanerId { get; set; }
        public LibraryUser? Loaner { get; set; }

        [Display(Name = "Book")]
        public int SelectedBookId { get; set; }

        // Navigatie-eigenschappen
        public Book Book { get; set; }

    }
}
