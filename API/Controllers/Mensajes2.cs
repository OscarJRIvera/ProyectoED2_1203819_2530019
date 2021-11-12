using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BD;
using API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Mensajes2 : Controller
    {
        private readonly Models.Data.Singleton F = Models.Data.Singleton.Instance;
        UsuarioCollection db = new UsuarioCollection();
        ContactosCollection db2 = new ContactosCollection();
        MensajesCollection db3 = new MensajesCollection();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost("Mensajes")]
        public IActionResult BuscarMensajes(Contactos Chat)
        {
            List<Mensajes> temp = new List<Mensajes>();
            string temp2 = "";
            if (Chat.tipo == true)
            {
               temp = db3.BuscarMensajesGrupo(Chat.Id);
            }
            else
            {
                temp2 = Chat.Usuario + Chat.Usuario;
                temp = db3.BuscarMensajesE(temp2);
                    
            }
            return Ok(temp);

        }
        [HttpPost("MandarMensaje")]
        public IActionResult MandarMensaje(Mensajes Msg)
        {
            db3.AgregarMen(Msg);
            return Ok();
        }

    }
}
