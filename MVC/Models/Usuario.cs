using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class Usuarios
    {
        [Key]
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Contraseña { get; set; }
        [Required]
        public int edad { get; set; }
        [Required]
        public DateTime fecha { get; set; }
        public int PublickKey { get; set; }
        public int SecretRandom { get; set; }

    }
}
