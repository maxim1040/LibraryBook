using LibraryBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryBook.ApiModels;

namespace LibraryBook.Areas.Data
{
    public class ApplicationDbContext : IdentityDbContext<LibraryUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; } = default!;
        public DbSet<Loan> Loans { get; set; } = default!;
        public DbSet<LibraryUser> Users { get; set; } = default!;
        public DbSet<Language> Languages { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Loaner)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.LoanerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Loans)
                .WithOne(l => l.Book)
                .HasForeignKey(l => l.BookId);

            modelBuilder.Entity<LibraryUser>()
                .HasMany(u => u.Loans)
                .WithOne(l => l.Loaner)
                .HasForeignKey(l => l.LoanerId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<LibraryBook.ApiModels.LoginModel> LoginModel { get; set; } = default!;
    }
}
