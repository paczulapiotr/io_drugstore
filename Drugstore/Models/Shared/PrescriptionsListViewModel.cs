using Drugstore.Core;
using Drugstore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class PrescriptionsListViewModel
    {
        public List<PrescriptionViewModel> Prescriptions { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}
