using System.ComponentModel.DataAnnotations;

namespace Insta_Blog.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Zəhmət olmasa mail daxil edin")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa şifrəni daxil edin")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
