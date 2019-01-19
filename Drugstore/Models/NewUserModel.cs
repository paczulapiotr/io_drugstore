using Drugstore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class NewUserModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public UserRoleTypes Role{ get; set; }
        public int DepartmentID { get; set; }

    }
}
