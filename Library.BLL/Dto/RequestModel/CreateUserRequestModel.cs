using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL.Dto.RequestModel
{
    public class CreateUserRequestModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string[] Roles { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class UpdateUserRequestModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
