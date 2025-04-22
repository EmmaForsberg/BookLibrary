using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Romance" },
                new Genre { Id = 2, Name = "Fantasy" },
                new Genre { Id = 3, Name = "Horror" }
                );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Cinderella", Author = "Disney", IsRead = false, Description = "A princess...", GenreId = 1 },
                new Book { Id = 2, Title = "Harry Potter", Author = "J K Rowling", IsRead = true, Description = "A wizard...", GenreId = 2 },
                new Book { Id = 3, Title = "1964", Author = "Stephen King", IsRead = true, Description = "A murder...", GenreId = 3 }
                );
        }
    }
}
