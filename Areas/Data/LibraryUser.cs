using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LibraryBook.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryBook.Areas.Data
{
    public class LibraryUser:IdentityUser

    {
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Display(Name = "Familienaam")]
        public string LastName { get; set; }

        public User User { get; set; }
        public List<Loan> Loans { get; set; }


    }
}
