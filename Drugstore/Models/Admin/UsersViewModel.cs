using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class UsersViewModel
    {
        public PaginationViewModel Pagination { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}
