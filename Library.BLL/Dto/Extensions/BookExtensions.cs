using Library.BLL.Dto.ResponseModel;
using Library.DAL.Entities;

namespace Library.BLL.Dto.Extensions
{
    public static class BookExtensions
    {
        public static User_BookReponseModel ToUserModel(this Book model)
        {
            if (model == null)
                return null;

            return new User_BookReponseModel()
            {
                Author = model.Author,
                ISBN = model.ISBN,
                Title = model.Title,
                Id = model.Id
            };
        }

        public static Admin_BookResponseModel ToAdminModel(this Book model)
        {
            if (model == null)
                return null;

            return new Admin_BookResponseModel()
            {
                Author = model.Author,
                ISBN = model.ISBN,
                Title = model.Title,
                Id = model.Id,
                IsAvailable = model.IsAvailable,
            };
        }

        public static Admin_BookResponseModel ToAdminModel(this Book model, int id)
        {
            if (model == null)
                return null;

            return new Admin_BookResponseModel()
            {
                Author = model.Author,
                ISBN = model.ISBN,
                Title = model.Title,
                Id = id,
                IsAvailable = model.IsAvailable,
            };
        }
    }
}
