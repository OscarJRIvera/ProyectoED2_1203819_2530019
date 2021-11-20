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
        [HttpPost("BuscarMensajes")]
        public IActionResult BuscarMensajes(Mensajes Chat)
        {
            List<Mensajes> temp = new List<Mensajes>();
            List<Mensajes> temp2 = new List<Mensajes>();

            if (Chat.GNombre != null)
            {
                temp = db3.BuscarMensajesGrupo(Chat);
                foreach (var k in temp)
                {
                    UsuariosModel UsuarioE = new UsuariosModel();
                    UsuarioE.Usuario = k.UsuarioE;
                    UsuarioE = db.Buscar(UsuarioE);
                    if (k.Texto != null)
                    {
                        k.Texto = F.CifrarRSA.Decifrar(k.Texto, UsuarioE.PublickKey);
                    }
                    temp2.Add(k);
                }
            }
            else
            {
                temp = db3.BuscarMensajesE(Chat);
                foreach (var k in temp)
                {
                    UsuariosModel UsuarioR = new UsuariosModel();
                    UsuarioR.Usuario = k.UsuarioR;
                    UsuarioR = db.Buscar(UsuarioR);
                    int llaveencomun = F.Llaves.SecretKey(UsuarioR.SecretRandom, k.publickey1);
                    if (k.Texto != null)
                    {
                        k.Texto = F.CifrarSDES.Decifrar(k.Texto, llaveencomun);
                    }
                    temp2.Add(k);
                }
            }

            return Ok(temp);

        }
        [HttpPost("ListaMensajesTotal")]
        public IActionResult ListaMensajesTotal(Mensajes Msg)
        {
            var lista = db3.Listatotal();
            return Ok(lista);
        }
        [HttpPost("MandarMensaje")]
        public IActionResult MandarMensaje(Mensajes Msg)
        {
            if (Msg.GNombre != null)
            {
                if (Msg.Texto != null)
                {
                    Msg.Texto = F.CifrarRSA.Cifrar(Msg.Texto, Msg.publickey1);
                }
                db3.AgregarMen(Msg);
            }
            else
            {
                if (Msg.Texto != null)
                {
                    int llaveencomun = llaveEnComun(Msg.UsuarioE, Msg.UsuarioR);
                    Msg.Texto = F.CifrarSDES.Cifrar(Msg.Texto, llaveencomun);
                }
                db3.AgregarMen(Msg);
            }
            return Ok();
        }
        [HttpPost("EliminarSolo")]
        public IActionResult EliminarSolo(Mensajes Msg)
        {
            List<Mensajes> templista;
            if (Msg.GNombre == "" || Msg.GNombre == null)
            {
                int llaveencomun = llaveEnComun(Msg.UsuarioE, Msg.UsuarioR);
                Msg.Texto = F.CifrarSDES.Cifrar(Msg.Texto, llaveencomun);
                templista = db3.BuscarMensaje(Msg);
                foreach (var k in templista)
                {
                    string fech = Convert.ToString(Msg.Fecha.Date) + Msg.Fecha.Second + Msg.Fecha.Minute + Msg.Fecha.Hour;
                    string fech2 = Convert.ToString(k.Fecha.Date) + k.Fecha.Second + k.Fecha.Minute + k.Fecha.Hour;
                    if (fech == fech2)
                    {
                        Msg.Id = k.Id;
                        Msg.publickey1 = k.publickey1;
                        Msg.Esconder = true;
                        db3.EliminarMensajeSolo(Msg, k.Id);
                    }
                }
            }
            else
            {
                templista = db3.BuscarMensajeGrupo(Msg);
                foreach (var k in templista)
                {
                    string verificar = F.CifrarRSA.Decifrar(k.Texto, k.publickey1);
                    if (verificar == Msg.Texto)
                    {
                        string fech = Convert.ToString(Msg.Fecha.Date) + Msg.Fecha.Second + Msg.Fecha.Minute + Msg.Fecha.Hour;
                        string fech2 = Convert.ToString(k.Fecha.Date) + k.Fecha.Second + k.Fecha.Minute + k.Fecha.Hour;
                        if (fech == fech2)
                        {
                            Msg.Id = k.Id;
                            Msg.publickey1 = k.publickey1;
                            Msg.Usuarios = k.Usuarios;
                            Msg.Texto = F.CifrarRSA.Cifrar(Msg.Texto, Msg.publickey1);
                            Msg.Esconder = true;
                            db3.EliminarMensajeSolo(Msg, k.Id);
                        }
                    }

                }
            }



            return Ok();
        }
        [HttpPost("EliminarTodos")]
        public IActionResult EliminarTodos(Mensajes Msg)
        {
            List<Mensajes> templista;
            int llaveencomun;
            if (Msg.GNombre == "" || Msg.GNombre == null)
            {
                llaveencomun = llaveEnComun(Msg.UsuarioE, Msg.UsuarioR);
                Msg.Texto = F.CifrarSDES.Cifrar(Msg.Texto, llaveencomun);
                templista = db3.BuscarMensaje(Msg);
                List<ObjectId> temp = new List<ObjectId>();
                foreach (var k in templista)
                {
                    string fech = Convert.ToString(Msg.Fecha.Date) + Msg.Fecha.Second + Msg.Fecha.Minute + Msg.Fecha.Hour;
                    string fech2 = Convert.ToString(k.Fecha.Date) + k.Fecha.Second + k.Fecha.Minute + k.Fecha.Hour;
                    if (fech == fech2)
                    {
                        Msg.Id = k.Id;
                        Msg.publickey1 = k.publickey1;
                        temp.Add(k.Id);
                        db3.EliminarMensajeTodos(temp);
                    }
                }

            }
            else
            {

                templista = db3.BuscarMensajeGrupo(Msg);
                List<ObjectId> temp = new List<ObjectId>();
                foreach (var k in templista)
                {
                    string verificar = F.CifrarRSA.Decifrar(k.Texto, k.publickey1);
                    if (verificar == Msg.Texto)
                    {
                        string fech = Convert.ToString(Msg.Fecha.Date) + Msg.Fecha.Second + Msg.Fecha.Minute + Msg.Fecha.Hour;
                        string fech2 = Convert.ToString(k.Fecha.Date) + k.Fecha.Second + k.Fecha.Minute + k.Fecha.Hour;
                        if (fech == fech2)
                        {
                            Msg.Id = k.Id;
                            Msg.publickey1 = k.publickey1;
                            temp.Add(k.Id);
                            db3.EliminarMensajeTodos(temp);
                        }
                    }
                }

            }

            return Ok();
        }
        public int llaveEnComun(string UsuarioE1, string UsuarioR1)
        {
            UsuariosModel UsuarioE = new UsuariosModel();
            UsuarioE.Usuario = UsuarioE1;
            UsuarioE = db.Buscar(UsuarioE);

            UsuariosModel UsuarioR = new UsuariosModel();
            UsuarioR.Usuario = UsuarioR1;
            UsuarioR = db.Buscar(UsuarioR);
            int llaveencomun = F.Llaves.SecretKey(UsuarioE.SecretRandom, UsuarioR.PublickKey);
            return llaveencomun;
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
