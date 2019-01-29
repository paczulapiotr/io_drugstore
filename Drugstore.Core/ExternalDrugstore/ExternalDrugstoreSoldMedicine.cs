using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Drugstore.Core
{
    public class ExternalDrugstoreSoldMedicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public MedicineOnStock StockMedicine { get; set; }
        public DateTime Date { get; set; }
        public int SoldQuantity { get; set; }
        public float PricePerOne { get; set; }
    }
}
