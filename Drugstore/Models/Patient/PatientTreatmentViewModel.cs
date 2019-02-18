using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class PatientTreatmentViewModel
    {
        public int Id { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public double TotalCost { get; set; }
        public List<PrescriptionGeneralDataModel> Prescriptions { get; set; }
        public bool IsValid { get; set; } = true;
        public string Error { get; set; } = "";
    }
}
