using System.ComponentModel.DataAnnotations;

namespace Library.BLL.Dto.RequestModel
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email cannot be empty"), MinLength(3, ErrorMessage = "Email is of invalid length")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password cannot be empty"), MinLength(1, ErrorMessage = "Password is of invalid length")]
        public string Password { get; set; }
    }
}