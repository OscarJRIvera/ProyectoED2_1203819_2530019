using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class ArchivosValores
    {
        public string Nombre { get; set; }
        public string NombreOriginal { get; set; }
        public byte[] archivo { get; set; }
    }
}
