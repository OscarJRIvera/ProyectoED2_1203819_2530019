using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MVC.Models
{
    public class Contacto
    {
        public Contacto()
        {
            Usuarios = new List<string>();
        }
        public bool tipo { get; set; }
        public string nombre { get; set; }
        public int num { get; set; }
        public string Usuario { get; set; }
        public string Usuario2 { get; set; }
        public bool estado { get; set; }
        public List<string> Usuarios { get; set; }
    }
}
