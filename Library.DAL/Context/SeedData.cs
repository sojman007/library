using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DAL.Entities;
using Library.DAL.Repositories.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Library.DAL.Context
{
    public static class SeedData
    {
        public static async Task Initialize(LibraryDbContext dbContext, LibraryUserDbContext userDbContext)
        {
            await SeedUsers(userDbContext);
            await SeedBooks(dbContext);
        }

        private static async Task SeedUsers(LibraryUserDbContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                var userList = new List<ApplicationUser>()
                {
                    new ApplicationUser() { Email = "admin@library.com", IsAdmin = true, Salt = "ONlnO9G4/rtDEHqSUYJ1Sw==", PasswordHash = "ONlnO9G4/rtDEHqSUYJ1Sw==", Name = "admin", },
                    new ApplicationUser() { Email = "user@library.com", IsAdmin = false, Salt = "ONlnO9G4/rtDEHqSUYJ1Sw==", PasswordHash = "ONlnO9G4/rtDEHqSUYJ1Sw==", Name = "user", },
                };

                dbContext.Users.AddRange(userList);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedBooks(LibraryDbContext dbContext)
        {
            if (!dbContext.Books.Any())
            {
                var bookList = new List<Book>()
                {
                    new Book { Author = "James Hardley Chase", Title = "Ace Up My Sleeve", ISBN = "1234567890", IsAvailable = true, },
                    new Book { Author = "Ben Murray Bruce", Title = "Common Sense", ISBN = "1234509876", IsAvailable = true, },
                    new Book { Author = "J.K. Rowling", Title = "Half Blood Prince", ISBN = "0987654321", IsAvailable = true, },
                };
                dbContext.Books.AddRange(bookList);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
