using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.FauxDb.Entities
{
    public class UserEntity
    {
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
    }
}
