using Drugstore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models.Shared
{
    public class PrescriptionViewModel
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public double TotalCost { get; set; }
        public DateTime CreationTime { get; set; }
        public VerificationState VerificationState { get; set; }
        public List<MedicineViewModel> AssignedMedicines { get; set; }
    }
}
