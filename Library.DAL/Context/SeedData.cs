using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DAL.Entities;

namespace Library.DAL.Context
{
    public static class SeedData
    {
        public static async Task Initialize(LibraryUserDbContext userDbContext, LibraryDbContext dbContext)
        {

        }

        private static async Task SeesUsers(LibraryUserDbContext dbContext)
        {
            var userList = new List<ApplicationUser>()
            {
                new ApplicationUser(){ Email = "admin@library.com", IsAdmin = true, Salt = "ONlnO9G4/rtDEHqSUYJ1Sw==", PasswordHash = "ONlnO9G4/rtDEHqSUYJ1Sw==", Name = "admin", },
                new ApplicationUser(){ Email = "user@library.com", IsAdmin = false, Salt = "ONlnO9G4/rtDEHqSUYJ1Sw==", PasswordHash = "ONlnO9G4/rtDEHqSUYJ1Sw==", Name = "user", },
            };

            
        }
    }
}
