using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drugstore.Core
{
    [Table("AssignedMedicines")]
    public class AssignedMedicine
    {
        [Key]
        public int ID { get; set; }

        public MedicineOnStock StockMedicine { get; set; }
        public uint AssignedQuantity { get; set; }
    }
}