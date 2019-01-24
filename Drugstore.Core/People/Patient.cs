using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drugstore.Core
{
    public class Patient : Person
    {
        public ICollection<MedicalPrescription> TreatmentHistory { get; set; } = new HashSet<MedicalPrescription>();

        [NotMapped]
        public string FullName { get => FirstName + " " + SecondName; }
    }
}