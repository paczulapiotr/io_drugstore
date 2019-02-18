using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Drugstore.Core
{

    public class ExternalDrugstoreMedicine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public MedicineOnStock StockMedicine { get; set; }
        public uint Quantity { get; set; }
        public double PricePerOne { get; set; }
    }
}
