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
        public int? StockId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float? PricePerOne { get; set; }
        public bool? IsRefunded { get; set; }
        public bool? IsNew { get; set; }
        public MedicineCategory? Category { get; set; }


        public bool ShouldSerializeStockId()
        {
            return StockId.HasValue;
        }
        public bool ShouldSerializePricePerOne()
        {
            return PricePerOne.HasValue;
        }
        public bool ShouldSerializeIsRefunded()
        {
            return IsRefunded.HasValue;
        }
        public bool ShouldSerializeIsNew()
        {
            return IsNew.HasValue;
        }
        public bool ShouldSerializeCategory()
        {
            return Category.HasValue;
        }
    
        

    }
}
