using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BD;
using API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Usuarios : ControllerBase
    {
        private readonly Models.Data.Singleton F = Models.Data.Singleton.Instance;
        UsuarioCollection db = new UsuarioCollection();
        ContactosCollection db2 = new ContactosCollection();
        // GET: api/<Usuarios>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost("Busqueda")]
        public IActionResult Busqueda(UsuariosModel Usuario)
        {
            var usuarioencontrado = db.Buscar(Usuario);
            return Ok(usuarioencontrado);
        }
        [HttpPost("Contactos")]
        public IActionResult Contactos(UsuariosModel Usuario)
        {
            List<Contactos> respuesta = db2.Buscar(Usuario);
            return Ok(respuesta);

        }
        [HttpPost("BusquedaContacto")]
        public IActionResult BusquedaContacto(Contactos chat)
        {
            Contactos temp = new Contactos();
            temp = db2.Buscar2(chat);
            return Ok(temp);

        }
        [HttpPost("ActualizarContacto")]
        public IActionResult ActualizarContacto(Contactos Contacto)
        {
            Contactos temp = new Contactos();
            temp = db2.Buscar2(Contacto);
            Contacto.Id = temp.Id;
            db2.ActualizarCont(Contacto);
            return Ok();

        }
        [HttpPost("NuevoContacto")]
        public IActionResult NuevoContacto(Contactos Contacto)
        {
            db2.AgregarCont(Contacto);
            return Ok();

        }

        [HttpPost("Nuevo")]
        public IActionResult Registro(UsuariosModel Usuario)
        {
            try
            {
                Usuario.SecretRandom = db.cantidad() + 1;
                Usuario.PublickKey = F.Llaves.Publickey(Usuario.SecretRandom);
                var usuarioencontrado = db.Buscar(Usuario);
                if (usuarioencontrado != null)
                {
                    string json = "Este Usuario ya existe";
                    return BadRequest(json);
                }
                Usuario.Contraseña = F.CifrarSDES.Cifrar(Usuario.Contraseña, 50);
                return Ok(db.insertar(Usuario));
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("Ingreso")]
        public IActionResult Ingreso(UsuariosModel Usuario)
        {
            try
            {
                var usuarioencontrado = db.Buscar(Usuario);
                if (usuarioencontrado == null)
                {
                    string json = "Usuario No existe";
                    return BadRequest(json);
                }
                if (usuarioencontrado.Contraseña == F.CifrarSDES.Cifrar(Usuario.Contraseña, 50))
                {
                    string json = "Exito";
                    return Ok(json);
                }
                else
                {
                    string json = "Usuario No existe";
                    return BadRequest(json);
                }
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("ContactoGrupo")]
        public IActionResult ContactoGrupo(Contactos Grupo)
        {
            List<Contactos> temp = new List<Contactos>();
            temp = db2.BuscarGrupo(Grupo);
            return Ok(temp);
        }
        [HttpPost("RetornarGrupo")]
        public IActionResult RetornarGrupo(Contactos Grupo)
        {
            var x = db2.BuscarGrupoEsp(Grupo);
            return Ok(x);
        }
        [HttpPost("BuscuarGruposUsuario")]
        public IActionResult BuscuarGruposUsuario(UsuariosModel User)
        {
            List<Contactos> temp = new List<Contactos>();
            temp = db2.BuscarGruposUsuario(User.Usuario);
            return Ok(temp);
        }
        [HttpPost("ActualizarContactoGrupo")]
        public IActionResult ActualizarContactoGrupo(Contactos valor)
        {
            var x = db2.BuscarGrupoEsp(valor);
            valor.Id = x.Id;
            db2.ActualizarContactoGrupo(valor);
            return Ok();
        }

    }
}
