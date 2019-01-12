using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Drugstore.Core
{
    public class Medicine
    {
        [Key]
        public int MedicineID { get; set; }
        public uint Quantity { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public float Price { get; set; }

    }
}
