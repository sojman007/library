using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DAL.Context;
using Library.DAL.Entities;
using Library.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Repositories
{
    public class UserRoleRepo : BaseRepo<UserRole>, IUserRoleRepo
    {
        public UserRoleRepo(LibraryUserDbContext context) : base(context)
        {
        }
    }
}
