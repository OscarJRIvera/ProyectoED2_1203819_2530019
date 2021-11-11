using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class Contacto
    {
        [Key]
        public bool tipo { get; set; }
        public string nombre { get; set; }
        public string Usuario { get; set; }
        public string Usuario2 { get; set; }
        public bool estado { get; set; }
    }
}
