using System.Collections.Generic;

namespace Drugstore.Core
{
    public class Patient : Person
    {
        public ICollection<MedicalPrescription> TreatmentHistory { get; set; }
    }
}