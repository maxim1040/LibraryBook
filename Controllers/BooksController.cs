using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryBook.Areas.Data;
using LibraryBook.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibraryBook.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult TestError()
        {
            throw new Exception("This is a test exception.");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StopLoan(int id)
        {
            // Retrieve the book along with its associated loans
            var book = await _context.Books
                .Include(b => b.Loans)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound(); // Book not found
            }

            // Get the current user
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID

            // Ensure the current user is the one who loaned the book or is an admin
            var activeLoan = book.Loans.FirstOrDefault(l => l.LoanerId == currentUserId || User.IsInRole("Admin"));

            if (activeLoan == null)
            {
                return Forbid(); // User is not the one who loaned the book
            }

            // Remove the active loan
            _context.Loans.Remove(activeLoan);

            // Update the book's loan status
            book.IsLoaned = false;
            book.LibraryUserId = null; // Clear the loaner from the book

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Books");
        }



        // GET: Books
        public async Task<IActionResult> Index(string searchField)
        {
            var usersTable = HttpContext.Items["UsersTable"] as IQueryable<LibraryUser>;

            // Get the books and include the loaner information via the middleware
            var books = from b in _context.Books
                        select new Book
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            ISBN = b.ISBN,
                            IsLoaned = b.IsLoaned,
                            LibraryUserId = b.LibraryUserId,
                            Loaner = usersTable.FirstOrDefault(u => u.Id == b.LibraryUserId) // Get the loaner from the middleware
                        };

            // Apply search filter if needed
            if (!string.IsNullOrEmpty(searchField))
            {
                books = books.Where(b => b.Title.Contains(searchField));
            }

            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersTable = HttpContext.Items["UsersTable"] as IQueryable<LibraryUser>;

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            // Get the loaner from the middleware
            book.Loaner = usersTable.FirstOrDefault(u => u.Id == book.LibraryUserId);

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var users = _context.Users.ToList();
            ViewData["LoanerUserName"] = new SelectList(users, "Id", "UserName");

            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,ISBN,Loans")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.IsLoaned = false;
                book.LibraryUserId = null;
                book.Loans = new List<Loan>();

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["LoanerUserName"] = new SelectList(_context.Users, "Id", "UserName", book.LibraryUserId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            ViewData["LoanerUserName"] = new SelectList(_context.Users, "Id", "Id", book.LibraryUserId);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ISBN,IsLoaned,LoanerUserName,Loans")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["LoanerUserName"] = new SelectList(_context.Users, "Id", "Id", book.LibraryUserId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersTable = HttpContext.Items["UsersTable"] as IQueryable<LibraryUser>;

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            // Get the loaner from the middleware
            book.Loaner = usersTable.FirstOrDefault(u => u.Id == book.LibraryUserId);

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
