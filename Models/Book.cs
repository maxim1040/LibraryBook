using LibraryBook.Areas.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;


namespace LibraryBook.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        [StringLength(100, ErrorMessage = "Dit kan niet langer zijn dan 100 karakters")]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        [StringLength(100, ErrorMessage = "Dit kan niet langer zijn dan 100 karakters")]
        [Display(Name = "Auteur")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        [StringLength(13, ErrorMessage = "Dit kan niet langer zijn dan 13 karakters")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Display(Name = "Loan Status")]
        public bool IsLoaned { get; set; }

        [ForeignKey("Loaner")]
        [Display(Name = "Geleend door")]
        public string? LibraryUserId { get; set; }
        public LibraryUser? Loaner { get; set; }

        // Navigatie-eigenschappen
        public List<Loan> Loans { get; set; }

    }
}

