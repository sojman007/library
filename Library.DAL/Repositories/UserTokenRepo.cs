using Library.DAL.Context;
using Library.DAL.Entities;
using Library.DAL.Repositories.Interface;

namespace Library.DAL.Repositories
{
    public class UserTokenRepo : BaseRepo<UserTokens>, IUserTokenRepo
    {
        public UserTokenRepo(LibraryUserDbContext context) : base(context)
        { }
    }
}
