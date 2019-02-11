using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Drugstore.Core
{
    public class Department
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Wprowadź nazwę oddziału")]
        [MinLength(10, ErrorMessage = "Nazwa oddziału powinna być dłuższa")]
        [MaxLength(50,ErrorMessage = "Nazwa oddziału powinna być krótsza")]
        public string Name { get; set; }

    }
}
