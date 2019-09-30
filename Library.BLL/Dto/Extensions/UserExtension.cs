using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Library.BLL.Dto.ResponseModel;
using Library.DAL.Entities;

namespace Library.BLL.Dto.Extensions
{
    public static class UserExtension
    {
        public static UserResponseModel ToModel(this ApplicationUser model, long id)
        {
            if (model == null)
                return null;

            return new UserResponseModel
            {
                Name = model.Name,
                Email = model.Email,
                IsAdmin = model.IsAdmin,
                Id = id
            };
        }

        public static UserResponseModel ToModel(this ApplicationUser model)
        {
            if (model == null)
                return null;

            return new UserResponseModel
            {
                Name = model.Name,
                Email = model.Email,
                IsAdmin = model.IsAdmin,
                Id = model.Id,
            };
        }
    }
}
