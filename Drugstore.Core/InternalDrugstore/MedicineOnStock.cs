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

        [MaxLength(50)]
        public string Name { get; set; }
        public MedicineCategory MedicineCategory { get; set; }
        public bool IsRefunded { get; set; }
        public uint Quantity { get; set; }
        public float PricePerOne { get; set; }

    }
}
