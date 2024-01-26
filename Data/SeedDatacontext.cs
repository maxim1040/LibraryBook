using LibraryBook.Areas.Data;
using LibraryBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryBook.Data
{
    public class SeedDatacontext
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<LibraryUser> userManager)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();    // Ensure that the database exists

                LibraryUser user1 = null;
                LibraryUser user2 = null;

                if (!context.Users.Any())
                {
                    user1 = new LibraryUser
                    {
                        FirstName = "System",
                        LastName = "Administrator",
                        UserName = "Admin",
                        Email = "System.Administrator@LibraryBook.be",
                        EmailConfirmed = true
                    };

                    user2 = new LibraryUser
                    {
                        FirstName = "User",
                        LastName = "UserLastName",
                        UserName = "User",
                        Email = "User@LibraryBook.be",
                        EmailConfirmed = true
                    };

                    // Assign values to user1.Id and user2.Id
                    await userManager.CreateAsync(user1, "Test123.");
                    await userManager.CreateAsync(user2, "Test123.");

                    if (!context.Books.Any())
                    {
                        var book1 = new Book
                        {
                            Title = "Harry Potter and the Philosopher's Stone",
                            Author = "J.K. Rowling",
                            ISBN = "9780747532743",
                            IsLoaned = true,
                            LoanerUserName = user1.UserName  // Use UserName instead of Id
                        };

                        var book2 = new Book
                        {
                            Title = "Moby Dick",
                            Author = "Herman Melville",
                            ISBN = "9780747538493",
                            IsLoaned = false,
                            LoanerUserName = null
                        };
                        context.Books.AddRange(book1, book2);
                        await context.SaveChangesAsync();
                    }

                    if (!context.Roles.Any())
                    {
                        context.Roles.AddRange(
                            new IdentityRole { Id = "User", Name = "User", NormalizedName = "user" },
                            new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "admin" });

                        context.UserRoles.AddRange(
                            new IdentityUserRole<string> { UserId = user1.Id, RoleId = "Admin" },
                            new IdentityUserRole<string> { UserId = user2.Id, RoleId = "User" });

                        await context.SaveChangesAsync(); // Save changes asynchronously
                    }

                }

            }
        }
    }
}
