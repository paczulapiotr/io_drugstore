using Drugstore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Drugstore.Models
{
    public class UserViewModel
    {
        public string SystemUserId { get; set; }

        [Required(ErrorMessage = "Pole nie może być puste")]
        [MinLength(3, ErrorMessage = "Pole powinno mieć co najmniej 2 znaki")]
        [MaxLength(30, ErrorMessage = "Pole powinno mieć co najwyżej 30 znaków")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole nie może być puste")]
        [MinLength(3, ErrorMessage = "Pole powinno mieć co najmniej 3 znaki")]
        [MaxLength(30, ErrorMessage = "Pole powinno mieć co najwyżej 30 znaków")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Pole nie może być puste")]
        [MinLength(5, ErrorMessage = "Nazwa użytkownika powinna być dłuższa")]
        [MaxLength(30, ErrorMessage = "Nazwa użytkownika powinna być krótsza")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Pole nie może być puste")]
        [MinLength(3, ErrorMessage = "Pole powinno mieć co najmniej 5 znaków")]
        [MaxLength(30, ErrorMessage = "Pole powinno mieć co najwyżej 30 znaków")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole nie może być puste")]
        [MinLength(5, ErrorMessage ="Hasło powinno mieć co najmniej 5 znaków")]
        [MaxLength(30, ErrorMessage = "Hasło powinno mieć co najwyżej 30 znaków")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole nie może być puste")]
        [Compare(nameof(Password), ErrorMessage ="Hasła się nie zgadzają")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Pole nie może być puste")]
        [StringLength(9,ErrorMessage = "Nieodpowiednia długość")]
        [RegularExpression("[0-9]*", ErrorMessage ="Nieodpowiedni format")]
        public string PhoneNumber { get; set; }

        public UserRoleTypes Role{ get; set; }
        public int DepartmentID { get; set; }

    }
}
