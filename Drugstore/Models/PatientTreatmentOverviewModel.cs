using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class PatientTreatmentOverviewModel
    {
        public int Id { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public float totalCost { get; set; }
        public List<PrescriptionGeneralDataModel> Prescriptions { get; set; }
        public bool IsValid { get; set; } = true;
        public string Error { get; set; } = "";
    }
}
