using Drugstore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class UserModifyData
    {
        public UserModel UserModel { get; set; }
        public List<Department> Departments{ get; set; }
    }
}
