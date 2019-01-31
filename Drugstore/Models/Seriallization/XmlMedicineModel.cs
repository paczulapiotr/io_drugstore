using Drugstore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Drugstore.Models.Seriallization
{
    [Serializable]
    public class XmlMedicineSupply
    {
        [XmlArray]
        public List<XmlMedicineModel> Medicines { get; set; } = new List<XmlMedicineModel>();
    }

    [Serializable]
    public class XmlMedicineModel
    {
        public bool? IsRefunded { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public int? StockId { get; set; }
        public bool? IsNew { get; set; }
        public MedicineCategory? Category { get; set; }
        public float? PricePerOne { get; set; }

    }
}
