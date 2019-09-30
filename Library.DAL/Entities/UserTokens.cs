using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Entities
{
    public class UserTokens : BaseEntity
    {
        public long UserId { get; set; }
        public string Token { get; set; }
    }
}
