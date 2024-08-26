using LibraryBook.Areas.Data;
using LibraryBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryBook.Data
{
    public class SeedDatacontext
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<LibraryUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            context.Database.EnsureCreated();    // Ensure that the database exists

            if (!context.Languages.Any())
            {
                context.AddRange(
                    new Language { Id = "- ", Name = "-", IsSystemLanguage = false, IsAvailable = DateTime.MaxValue },
                    new Language { Id = "en", Name = "English", IsSystemLanguage = true },
                    new Language { Id = "nl", Name = "Nederlands", IsSystemLanguage = true },
                    new Language { Id = "fr", Name = "français", IsSystemLanguage = true }
                );
                context.SaveChanges();
            }

            Language.GetLanguages(context);

            // Check if roles exist, if not, create them
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seeding Admin User
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                var adminUser = new LibraryUser
                {
                    FirstName = "System",
                    LastName = "Administrator",
                    UserName = "Admin",
                    Email = "System.Administrator@LibraryBook.be",
                    EmailConfirmed = true
                };

                // Create the Admin user with the password
                var result = await userManager.CreateAsync(adminUser, "Test123.");
                if (result.Succeeded)
                {
                    // Assign Admin role to the user
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seeding Regular User
            if (userManager.FindByNameAsync("User").Result == null)
            {
                var regularUser = new LibraryUser
                {
                    FirstName = "User",
                    LastName = "UserLastName",
                    UserName = "User",
                    Email = "User@LibraryBook.be",
                    EmailConfirmed = true
                };

                // Create the User with the password
                var result = await userManager.CreateAsync(regularUser, "Test123.");
                if (result.Succeeded)
                {
                    // Assign User role to the regular user
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }

            // Seeding Books
            if (!context.Books.Any())
            {
                var adminUser = await userManager.FindByNameAsync("Admin");

                context.Books.AddRange(
                    new Book
                    {
                        Title = "Harry Potter and the Philosopher's Stone",
                        Author = "J.K. Rowling",
                        ISBN = "9780747532743",
                        IsLoaned = true,
                        LibraryUserId = adminUser?.Id // Link to the admin user
                    },
                    new Book
                    {
                        Title = "Moby Dick",
                        Author = "Herman Melville",
                        ISBN = "9780747538493",
                        IsLoaned = false,
                        LibraryUserId = null // This book is not loaned
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
}
