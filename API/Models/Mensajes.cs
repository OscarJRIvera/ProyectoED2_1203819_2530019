using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Http;

namespace API.Models
{
    public class Mensajes
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string groupname { get; set; }
        public string UsuarioE { get; set; }
        public string UsuarioR { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set;}
        public IFormFile File { get; set; }
        public string FileString { get; set; }
    }
}
