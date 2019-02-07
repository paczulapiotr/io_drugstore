using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public bool IsRefunded { get; set; }

        [Required]
        public uint Quantity { get; set; }

        [Required]
        public float PricePerOne { get; set; }

    }
}
