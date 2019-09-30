using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.BLL.Dto.RequestModel;
using Library.BLL.Dto.ResponseModel;

namespace Library.BLL.Services.Interfaces
{
    public interface IBookService
    {
        Task<MiscResponse<Admin_BookResponseModel>> CreateBookAsync(CreateBookRequestModel model);
        Task<MiscResponse<object>> DeleteBookAsync(long bookId);
        SearchResponse<User_BookReponseModel> SearchForBooksAsNonAdmin(SearchBookRequestModel model, int page, int size);
        SearchResponse<Admin_BookResponseModel> SearchForBooksAsAdmin(SearchBookRequestModel model, int page, int size);
        Task<MiscResponse<User_BookReponseModel>> BorrowBookAsync(long bookId, string userEmail);
        Task<MiscResponse<User_BookReponseModel>> ReturnBookAsync(long bookId, string userEmail);
    }
}
