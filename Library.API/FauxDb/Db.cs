using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.API.FauxDb.Entities;

namespace Library.API.FauxDb
{
    public class Db
    {
        static Db()
        {
            UserTokens = new List<UserTokenEntity>();
            Books = new List<BookEntity>()
            {
                new BookEntity { author = "james hardley chase", isbn = "1234567890", name = "ace of spade"},
                new BookEntity { author = "j.k rowling", name = "half blood prince"},
                new BookEntity { author = "sidney sheldon", isbn = "6598713240", name = "dooms day"},
            };

            UserRoles = new List<UserRoleEntity>()
            {
                new UserRoleEntity{ email = "isiaq.oa@gmail.com", role = "admin" },
                new UserRoleEntity{ email = "isiaq.oa@gmail.com", role = "test" },
                new UserRoleEntity{ email = "oalashe@yahoo.com", role = "admin" },
                new UserRoleEntity{ email = "wale@hellocare.com", role = "admin" },
            };

            Users = new List<UserEntity>()
            {
                new UserEntity{ email = "isiaq.oa@gmail.com", name = "gmail", password = "loremIpsum123"},
                new UserEntity{ email = "oalashe@yahoo.com", name = "yahoo", password = "loremIpsum123"},
                new UserEntity{ email = "wale@hellocare.com", name = "hellocare", password = "loremIpsum123"},
            };
        }

        public static List<UserTokenEntity> UserTokens { get; set; }
        public static List<BookEntity> Books { get; set; }
        public static List<UserEntity> Users { get; set; }
        public static List<UserRoleEntity> UserRoles { get; set; }
    }
}
