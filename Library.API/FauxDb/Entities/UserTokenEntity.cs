using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.FauxDb.Entities
{
    public class UserTokenEntity
    {
        public string email { get; set; }
        public string token { get; set; }
    }
}
