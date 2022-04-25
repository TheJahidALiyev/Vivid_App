using System.ComponentModel.DataAnnotations;

namespace Insta_Blog.ViewModels
{
    public class AccountVM
    {
        [Required(ErrorMessage = "Zəhmət olmasa yeni şifrəni daxil edin")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa yeni şifrəni daxil edin")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ReNewPassword { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
