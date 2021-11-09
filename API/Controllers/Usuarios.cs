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
        UsuarioCollection db = new UsuarioCollection();
        // GET: api/<Usuarios>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost("Nuevo")]
        public IActionResult Registro(UsuariosModel Usuario)
        {
            try
            {
               var usuarioencontrado=  db.Buscar(Usuario);
                if (Usuario.Usuario != ""){
                    string json = "Este Usuario ya existe";
                    return BadRequest(json);
                }
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
                if (Usuario.Usuario == "")
                {
                    string json = "Usuario No existe";
                    return BadRequest(json);
                }
                if (usuarioencontrado.Contraseña == Usuario.Contraseña)
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
