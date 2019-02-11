using System.ComponentModel.DataAnnotations;
namespace Drugstore.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Wprowadź nazwę użytkownika")]
        [UIHint("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wprowadź hasło")]
        [UIHint("password")]
        public string Password { get; set; }

    }
}
