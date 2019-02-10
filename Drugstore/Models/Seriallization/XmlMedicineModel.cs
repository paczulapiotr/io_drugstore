using Drugstore.Core;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Drugstore.Models.Seriallization
{
    [Serializable]
    public class XmlMedicineSupplyModel
    {
        [XmlArray]
        public List<XmlMedicineModel> Medicines { get; set; } = new List<XmlMedicineModel>();
    }

    [Serializable]
    public class XmlMedicineModel
    {
        public int? StockId { get; set; }
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public float? PricePerOne { get; set; }
        public bool? IsRefunded { get; set; }
        public bool IsNew { get; set; } = false;
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
            return IsNew;
        }
        public bool ShouldSerializeCategory()
        {
            return Category.HasValue;
        }



    }
}
