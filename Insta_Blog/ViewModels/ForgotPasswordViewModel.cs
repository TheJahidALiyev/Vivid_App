using System.ComponentModel.DataAnnotations;

namespace Insta_Blog.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Don't leave empty"), EmailAddress(ErrorMessage = "Enter a valid e-mail.")]
        public string Email { get; set; }
    }
}
