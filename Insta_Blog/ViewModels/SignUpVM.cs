using System.ComponentModel.DataAnnotations;

namespace Insta_Blog.ViewModels
{
    public class SignUpVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        //[RegularExpression(@"[a-z]{3,4}", ErrorMessage = "Şifrə 8 simvoldan ibarət olmalıdır")]
        [Required(ErrorMessage = "Can't be empty"), DataType(DataType.Password)]

        public string Password { get; set; }
        [Required(ErrorMessage = "Can't be empty"), Compare(nameof(Password), ErrorMessage = "Enter valid password"), DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }
    }
}
