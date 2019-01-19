using Drugstore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class CreateNewUserData
    {
        public NewUserModel UserModel { get; set; }
        public List<Department> Departments{ get; set; }
    }
}
