using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Drugstore.Core
{
    public class Patient : Person
    {
        public ICollection<MedicalPrescription> TreatmentHistory { get; set; }
    }
}