using Book_Store.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Book_Store.Extensions
{
    internal static class DatabaseSeedDataExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            var accountRoles = new[]
            {
                new Role { Id = 1, Name = "admin" },
                new Role { Id = 2, Name = "user" },
                new Role { Id = 3, Name = "guest" },
            };

            var accounts = new[]
            {
                new Account { Id = 1, Login = "admin", Password = "admin", RoleId = accountRoles[0].Id },
            };

            var authors = new[]
            {
                new Author { Id = 1, FirstName = "Joanne", LastName = "Rowling" },
                new Author { Id = 2, FirstName = "Stephen", LastName = "King" },
                new Author { Id = 3, FirstName = "Fyodor", LastName = "Dostoevsky", Patronymic = "Mikhailovich" },
            };

            var publishers = new[] 
            {
                new Publisher { Id = 1, Name = "Bloomsbury Publishing" },
            };

            var genres = new[]
            {
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 2, Name = "Adventure" },
                new Genre { Id = 3, Name = "Romance" },
                new Genre { Id = 4, Name = "Dystopian" },
                new Genre { Id = 5, Name = "Horror" },
                new Genre { Id = 6, Name = "Thriller" },
                new Genre { Id = 7, Name = "Historical fiction" },
                new Genre { Id = 8, Name = "Science Fiction" },
                new Genre { Id = 9, Name = "Childrens" },
                new Genre { Id = 10, Name = "Art" },
                new Genre { Id = 11, Name = "Development" },
                new Genre { Id = 12, Name = "Self-help" },
                new Genre { Id = 13, Name = "Motivational" },
                new Genre { Id = 14, Name = "History" },
                new Genre { Id = 15, Name = "Travel" },
                new Genre { Id = 16, Name = "Humor" },
            };

            var books = new[]
            {
                new Book
                {
                    Id = 1,
                    Name = "Harry Potter and the Philosopher's Stone",
                    AuthorId = authors[0].Id,
                    PublisherId = publishers[0].Id,
                    Pages = 400,
                    GenreId = genres[0].Id,
                    YearPublishing = new DateTime(1997, 1, 1),
                    CostPrice = 30,
                    Price = 45.50M
                },
                new Book
                {
                    Id = 2,
                    Name = "Harry Potter and the Chamber of Secrets",
                    AuthorId = authors[0].Id,
                    PublisherId = publishers[0].Id,
                    Pages = 251,
                    GenreId = genres[0].Id,
                    YearPublishing = new DateTime(1998, 1, 1),
                    CostPrice = 20,
                    Price = 61.50M,
                },
            };

            var continuationBooks = new[]
            {
                new ContinuationBook { BookId = books[1].Id, PredecessorBookId = books[0].Id }
            };

            var discounts = new[]
            {
                new Discount { Id = 1, Name = "Harry Potter Discount", Percent = 70,
                    StartDate = new DateTime(2022, 3, 19), EndDate = new DateTime(2022, 12, 1) }
            };

            var bookDiscounts = new[]
            {
                new BookDiscount { DiscountId = discounts[0].Id, BookId = books[0].Id },
                new BookDiscount { DiscountId = discounts[0].Id, BookId = books[1].Id },
            };

            modelBuilder
                .Entity<Role>()
                .HasData(accountRoles);

            modelBuilder
                .Entity<Account>()
                .HasData(accounts);

            modelBuilder
                .Entity<Author>()
                .HasData(authors);

            modelBuilder
                .Entity<Publisher>()
                .HasData(publishers);

            modelBuilder
                .Entity<Genre>()
                .HasData(genres);

            modelBuilder
                .Entity<Book>()
                .HasData(books);

            modelBuilder
                .Entity<ContinuationBook>()
                .HasData(continuationBooks);

            modelBuilder
                .Entity<Discount>()
                .HasData(discounts);

            modelBuilder
                .Entity<BookDiscount>()
                .HasData(bookDiscounts);
        }
    }
}
