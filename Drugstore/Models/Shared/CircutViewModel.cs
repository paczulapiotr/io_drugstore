using System.Collections.Generic;
using Drugstore.Core;
using Newtonsoft.Json;

namespace Drugstore.Models
{
    public class CircutViewModel
    {
        public List<Patient> Patients { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}