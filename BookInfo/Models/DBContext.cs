using Microsoft.EntityFrameworkCore;
using BookInfo.ViewModels;

namespace BookInfo.Models

{
    public class DBContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Author> Authors { get; set; }  
        public DbSet<Book> Books { get; set; }  
        public DbSet<Genre> Genres { get; set; }    
        public DbSet<BooksGenre> BooksGenres { get; set; }
        public DbSet<FavoriteBooks> FavoriteBooks { get; set; }

        public DBContext(DbContextOptions<DBContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BooksGenre>().HasKey(table => new {
                table.GenreId,
                table.BookId
            });
            modelBuilder.Entity<FavoriteBooks>().HasKey(table => new {
                table.UserId,
                table.BookId
            });
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new() { Id = 1, Name = adminRoleName };
            Role userRole = new() { Id = 2, Name = userRoleName };
            User adminUser = new() { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<BookInfo.ViewModels.BookView>? BookView { get; set; }
    }
}
