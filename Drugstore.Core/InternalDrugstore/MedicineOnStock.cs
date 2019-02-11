using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Drugstore.Core
{
    public class MedicineOnStock
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public MedicineCategory MedicineCategory { get; set; }

        [Required]
        [Range(0.0d,1.0d)]
        public double Refundation { get; set; } = 0.0f;

        [Required]
        public uint Quantity { get; set; }

        [Required]
        public double PricePerOne { get; set; }

    }
}
