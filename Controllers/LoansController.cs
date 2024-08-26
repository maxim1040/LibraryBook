using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryBook.Models;
using Microsoft.AspNetCore.Authorization;
using LibraryBook.Areas.Data;
using Microsoft.AspNetCore.Identity;

namespace LibraryBook.Controllers
{
    
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<LibraryUser> _userManager;

        public LoansController(ApplicationDbContext context, UserManager<LibraryUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Loans
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string titleFilter, int selectedGroup)
        {
            var libraryBookContext = _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Loaner); // Include the Loaner (LibraryUser)

            return View(await libraryBookContext.ToListAsync());
        }


        // GET: Loans/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/Create
        [Authorize(Policy = "CreateLoanPolicy")]
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            return View();
        }

        // POST: Loans/Create
        [Authorize(Policy = "CreateLoanPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LoanDate,ReturnDate,Deleted,BookId")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                loan.LoanerId = user?.Id;
                loan.UserId = user?.Id;

                // Set IsLoaned status of the book to true
                var book = await _context.Books.FindAsync(loan.BookId);
                if (book != null)
                {
                    book.IsLoaned = true;
                    book.Loaner = user;
                    _context.Update(book);
                }
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Books");
            }

            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Author", loan.BookId);
            return View(loan);
        }

        // GET: Loans/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Author", loan.BookId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,LoanDate,ReturnDate,Deleted,BookId")] Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Author", loan.BookId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var loan = await _context.Loans.FindAsync(id);

            if (loan != null)
            {
                var bookId = loan.BookId;
                var book = await _context.Books.FindAsync(bookId);

                if (book != null)
                {
                    // Reset book properties
                    book.IsLoaned = false;
                    book.LibraryUserId = null;
                }

                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int? id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
