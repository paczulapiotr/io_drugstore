using Drugstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Data
{
    public interface IRepository
    {
        UserModel GetUser(string userId);
        void DeleteUser(string userId);
        void EditUser(UserModel updatedUser);
        void AddNewUser(UserModel newUser);
    }
}
