using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL.Dto.RequestModel
{
    public class SearchUserRequestModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
