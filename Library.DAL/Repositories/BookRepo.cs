using Library.DAL.Context;
using Library.DAL.Entities;
using Library.DAL.Repositories.Interface;

namespace Library.DAL.Repositories
{
    public class BookRepo : BaseRepo<Book>, IBookRepo
    {
        public BookRepo(LibraryDbContext context) : base(context)
        { }
    }
}
