using System.Collections.Generic;

namespace Drugstore.Core
{
    public class Doctor : Person
    {
        public ICollection<MedicalPrescription> IssuedPresciptions { get; set; } = new HashSet<MedicalPrescription>();
    }
}