using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BD;
using API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CIFRADO;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : Controller
    {
        private readonly Models.Data.Singleton F = Models.Data.Singleton.Instance;
        UsuarioCollection db = new UsuarioCollection();
        ContactosCollection db2 = new ContactosCollection();
        MensajesCollection db3 = new MensajesCollection();
        ArchivosCollection db4 = new ArchivosCollection();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost("Comprimir")]
        public IActionResult Comprimir(ArchivosValores Archivo)
        {

            return Ok();
        }
        [HttpPost("Descomprimir")]
        public IActionResult Descomprimir(ArchivosValores Archivo)
        {

            return Ok();
        }

    }
}
