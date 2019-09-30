using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.BLL.Dto.RequestModel;
using Library.BLL.Dto.ResponseModel;

namespace Library.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<MiscResponse<UserResponseModel>> CreateUserAsync(CreateUserRequestModel model);
        Task<MiscResponse<object>> DeleteUserAsync(long userId);
        Task<MiscResponse<UserResponseModel>> UpdateUserAsync(UpdateUserRequestModel model);
        SearchResponse<UserResponseModel> SearchForUser(SearchUserRequestModel model, int page, int size);
        UserResponseModel GetUserWithCredentials(string email, string password);
        bool TokenIsValid(string token);
        Task CreateUserToken(string token, long userId);
        Task InvalidateToken(string token);
        Task InvalidateAllToken(string userEmail);
    }
}
