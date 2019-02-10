using Drugstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Data
{
    public interface IRepository
    {
        UserViewModel GetUser(string userId);
        void DeleteUser(string userId);
        void EditUser(UserViewModel updatedUser);
        void AddNewUser(UserViewModel newUser);
    }
}
