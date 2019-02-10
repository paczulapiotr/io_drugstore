using Drugstore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models
{
    public class DoctorPrescriptionViewModel
    {
        public AssignedMedicine[] Medicines { get; set; }
        public Patient Patient { get; set; }
    }
}
