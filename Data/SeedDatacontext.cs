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
                Book book1 = null;
                Book book2 = null;

                if (!context.Languages.Any())
                {
                    context.AddRange(
                        new Language { Id = "- ", Name = "-", IsSystemLanguage = false, IsAvailable = DateTime.MaxValue },
                        new Language { Id = "en", Name = "English", IsSystemLanguage = true },
                        new Language { Id = "nl", Name = "Nederlands", IsSystemLanguage = true },
                        new Language { Id = "fr", Name = "français", IsSystemLanguage = true },
                        new Language { Id = "de", Name = "Deutsch", IsSystemLanguage = true }
                        );
                    context.SaveChanges();
                }

                Language.GetLanguages(context);

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

                    if (!context.Books.Any())
                    {
                        book1 = new Book
                        {
                            Title = "Harry Potter and the Philosopher's Stone",
                            Author = "J.K. Rowling",
                            ISBN = "9780747532743",
                            IsLoaned = true,
                            LibraryUserId = user1.Id    
                        };

                        book2 = new Book
                        {
                            Title = "Moby Dick",
                            Author = "Herman Melville",
                            ISBN = "9780747538493",
                            IsLoaned = false,
                            LibraryUserId = null
                        };

                        context.Books.AddRange(book1, book2);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
