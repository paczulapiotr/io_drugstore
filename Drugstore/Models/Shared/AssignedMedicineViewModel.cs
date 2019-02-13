using Drugstore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drugstore.Models.Shared
{
    public class MedicineViewModel
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public string Name { get; set; }
        public double PricePerOne { get; set; }
        public double Refundation { get; set; }
        public MedicineCategory MedicineCategory { get; set; }
        public int Quantity { get; set; }
    }
}
