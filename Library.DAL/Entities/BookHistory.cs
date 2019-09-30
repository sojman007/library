using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Library.DAL.Entities
{
    public class BookHistory : BaseEntity
    {
        public long BookId { get; set; }
        public long LenderId { get; set; }
        public bool Returned { get; set; }
    }
}
