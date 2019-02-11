using Drugstore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class DoctorTreatmentHistoryViewModel
    {
        public List<MedicalPrescription> Prescriptions { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}
