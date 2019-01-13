using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Drugstore.Core
{
    public abstract class Person
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int SystemUserID { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string SecondName { get; set; }
        public Department Department { get; set; }
    }
}
