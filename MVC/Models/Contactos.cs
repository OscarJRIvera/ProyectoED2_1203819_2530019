using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Contactos
    {
        [Key]
        [Required]
        public string Usuario { get; set; }
        public int PublickKey { get; set; }
    }
}
