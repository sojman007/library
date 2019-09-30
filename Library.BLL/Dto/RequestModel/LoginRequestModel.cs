using System.ComponentModel.DataAnnotations;

namespace Library.BLL.Dto.RequestModel
{
    public struct LoginRequestModel
    {
        [Required(ErrorMessage = "Email cannot be empty")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}