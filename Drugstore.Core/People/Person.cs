﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Drugstore.Identity;

namespace Drugstore.Core
{
    public abstract class Person
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string SecondName { get; set; }

        public Department Department { get; set; }
        public SystemUser SystemUser { get; set; }

        [NotMapped]
        public string FullName => FirstName + " " + SecondName;
    }
}