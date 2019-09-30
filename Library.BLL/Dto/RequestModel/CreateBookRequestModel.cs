using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL.Dto.RequestModel
{
    public struct CreateBookRequestModel
    {
        [Required (ErrorMessage = "Book title is required"), MinLength(1, ErrorMessage = "Book title cannot be less than 1 character")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Book author is required"), MinLength(3, ErrorMessage = "Book author cannot be less than 3 characters")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Book ISBN is required"), MinLength(5, ErrorMessage = "Book ISBN cannot be less than 5 characters")]
        public string ISBN { get; set; }
    }
}
