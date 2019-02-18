using Drugstore.Core;
using Drugstore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class DoctorPrescriptionViewModel
    {
        public MedicineViewModel [] Medicines { get; set; }
        public PatientViewModel Patient { get; set; }
    }
}
