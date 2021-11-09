using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    
    public class Solicitud
    {
        [Key]
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
