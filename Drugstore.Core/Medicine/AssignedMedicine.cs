using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Drugstore.Core
{
    [Table("AssignedMedicines")]
    public class AssignedMedicine
    {
        [Key]
        public int ID { get; set; }
        public MedicineOnStock StockMedicine { get; set; }
        public uint AssignedQuantity { get; set; }
        public float Cost { get; set; }
    }
}
