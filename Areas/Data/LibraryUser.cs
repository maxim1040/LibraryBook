using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryBook.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryBook.Areas.Data
{
    public class LibraryUser:IdentityUser

    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public User User { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
