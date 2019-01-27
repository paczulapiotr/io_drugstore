using System.ComponentModel.DataAnnotations;

namespace Drugstore.Core
{
    public class Department
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }
}