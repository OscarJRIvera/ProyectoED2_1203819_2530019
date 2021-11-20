using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Chats
    {
        [Key]
        public string Nombre { get; set; }

        public List<Solicitud> Usuarios = new List<Solicitud>();
    }
}
