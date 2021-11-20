using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MVC.Models
{
    public class Mensajes
    {
        public Mensajes()
        {
            Usuarios = new List<string>();
        }
        public string GNombre { get; set; }
        public int Num { get; set; }
        public string UsuarioE { get; set; }
        public string UsuarioR { get; set; }
        public bool Esconder { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public string FileStringOriginal { get; set; }
        public int publickey1 { get; set; }
        public List<string> Usuarios { get; set; }
        public string FileString { get; set; }
    }
}
