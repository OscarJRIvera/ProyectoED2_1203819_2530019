using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MVC.Models
{
    public class Mensajes
    {
        public string groupname { get; set; }
        public string UsuarioE { get; set; }
        public string UsuarioR { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public IFormFile File { get; set; }
        public string FileString { get; set; }
    }
}
