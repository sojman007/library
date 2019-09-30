using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.FauxDb.Entities
{
    public class BookEntity
    {
        public string name { get; set; }
        public string isbn { get; set; }
        public string author { get; set; }
    }
}
