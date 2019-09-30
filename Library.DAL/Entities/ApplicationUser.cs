using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DAL.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
    }
}
