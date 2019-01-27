using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drugstore.Core
{
    [Table("MedicalPrescriptions")]
    public class MedicalPrescription
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public Doctor Doctor { get; set; }

        [Required]
        public Patient Patient { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        public ICollection<AssignedMedicine> Medicines { get; set; }

        [Required]
        public VerificationState VerificationState { get; set; } = VerificationState.NotVerified;
    }
}