using System.ComponentModel.DataAnnotations;

namespace Insta_Blog.ViewModels
{
    public class UserResetPasswordModel
    {

        [Required(ErrorMessage = "Don't leave empty"), DataType(DataType.Password)]

        public string Password { get; set; }
        [Required(ErrorMessage = "Don't leave empty"), DataType(DataType.Password)]
        [Compare(nameof(Password))]

        public string ConfirmPassword { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }
    }
}
