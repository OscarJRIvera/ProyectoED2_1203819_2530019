using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BD;
using API.Models;
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
            Contactos temp = new Contactos();
            temp.Usuario = Usuario.Usuario;
            List<Contactos> respuesta = db2.Buscar(temp);
            return Ok(respuesta);

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
    }
}
