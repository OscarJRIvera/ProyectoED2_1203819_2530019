using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Models;
using MVC.Clases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Models.Data.Singleton F = Models.Data.Singleton.Instance;
        public IWebHostEnvironment x;
        public UsuariosController(IWebHostEnvironment env)
        {
            x = env;
        }
        public ActionResult EntradaUsuarioActivo(string nombre, int Num, string valor, string RespuestaAgregarGrupo, string VerficiarTextofiltro)
        {

            if (RespuestaAgregarGrupo != null)
            {
                ViewBag.RespuestaAgregarGrupo = RespuestaAgregarGrupo;
                ViewBag.Tipo = true;
            }
            Usuarios UsuarioActivo = UsuarioActivoFuncion();
            while (UsuarioActivo == null)
            {
                UsuarioActivo = UsuarioActivoFuncion();
            }
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Contactos", UsuarioActivo).Result;
            List<Contacto> listacontactos = response.Content.ReadAsAsync<List<Contacto>>().Result;

            List<Contacto> ListaCont = new List<Contacto>();
            foreach (var k in listacontactos)
            {
                if (k.estado == true)
                {
                    ListaCont.Add(k);
                }
            }
            Mensajes Temp = new Mensajes();
            List<Mensajes> mensajes = new List<Mensajes>();
            List<Mensajes> mensajes2 = new List<Mensajes>();
            if (nombre != null)
            {
                ViewBag.Tipo = true;
                HttpContext.Session.SetString("Mensajeu2", "");
                HttpContext.Session.SetString("NGrupo", nombre);
                HttpContext.Session.SetString("Num", Convert.ToString(Num));
                Temp.UsuarioE = UsuarioActivo.Usuario;
                Temp.GNombre = nombre;
                Temp.Num = Num;
                if (VerficiarTextofiltro != null)
                {
                    Temp.Texto = VerficiarTextofiltro;
                    response = RutaApi.Api.PostAsJsonAsync("Mensajes2/FiltroMensajesGrupo", Temp).Result;
                    mensajes = response.Content.ReadAsAsync<List<Mensajes>>().Result;
                }
                else
                {
                    response = RutaApi.Api.PostAsJsonAsync("Mensajes2/BuscarMensajes", Temp).Result;
                    mensajes = response.Content.ReadAsAsync<List<Mensajes>>().Result;
                }
            }
            else
            {
                if (valor != null)
                {
                    HttpContext.Session.SetString("NGrupo", "");
                    HttpContext.Session.SetString("Num", "");
                    HttpContext.Session.SetString("Mensajeu2", valor);
                    Temp.UsuarioE = UsuarioActivo.Usuario;
                    Temp.UsuarioR = valor;
                    if (VerficiarTextofiltro != null)
                    {
                        Temp.Texto = VerficiarTextofiltro;
                        response = RutaApi.Api.PostAsJsonAsync("Mensajes2/FiltroMensajes", Temp).Result;
                        mensajes = response.Content.ReadAsAsync<List<Mensajes>>().Result;
                    }
                    else
                    {
                        response = RutaApi.Api.PostAsJsonAsync("Mensajes2/BuscarMensajes", Temp).Result;
                        mensajes = response.Content.ReadAsAsync<List<Mensajes>>().Result;
                    }
                }
                else
                {
                    HttpContext.Session.SetString("NGrupo", "");
                    HttpContext.Session.SetString("Num", "");
                    HttpContext.Session.SetString("Mensajeu2", "");
                }
            }
            if (mensajes == null)
            {
                mensajes2 = new List<Mensajes>();
            }
            else
            {
                foreach (var mens in mensajes)
                {
                    if (mens.Esconder == true && mens.UsuarioE == UsuarioActivo.Usuario)
                    {

                    }
                    else
                    {
                        mensajes2.Add(mens);
                    }
                }
            }
            ViewBag.Mensajes = mensajes2;
            ViewBag.Contactos = ListaCont;
            ViewBag.Comprobar = UsuarioActivo.Usuario;

            return View("Entrada");
        }
        public ActionResult Solicitudes()
        {
            Usuarios UsuarioActivo = UsuarioActivoFuncion();
            //Usuarios UsuarioActivo = new Usuarios();
            //UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
            //HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
            while (UsuarioActivo == null)
            {
                UsuarioActivo = UsuarioActivoFuncion();
            }
            HttpResponseMessage response2 = RutaApi.Api.PostAsJsonAsync("Usuarios/Contactos", UsuarioActivo).Result;
            List<Contacto> listacontactos = response2.Content.ReadAsAsync<List<Contacto>>().Result;
            List<Contacto> listaespera = new List<Contacto>();
            foreach (var k in listacontactos)
            {
                if (k.estado == false && k.Usuario != UsuarioActivo.Usuario)
                {
                    listaespera.Add(k);
                }
            }
            ViewBag.Comprueba = UsuarioActivo.Usuario;
            ViewBag.Solicitudes = listaespera;
            ViewBag.Mensaje = HttpContext.Session.GetString("RespuestaAgregar");
            HttpContext.Session.SetString("RespuestaAgregar", "");
            return View("Solicitudes");
        }

        public ActionResult Agregar(string BuscarContacto)
        {
            Usuarios UsuarioActivo = UsuarioActivoFuncion();
            //Usuarios UsuarioActivo = new Usuarios();
            //UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
            //HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
            //UsuarioActivo = response.Content.ReadAsAsync<Usuarios>().Result;

            Usuarios UsuarioBuscar = new Usuarios();
            UsuarioBuscar.Usuario = BuscarContacto;
            HttpResponseMessage response2 = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioBuscar).Result;
            UsuarioBuscar = response2.Content.ReadAsAsync<Usuarios>().Result;
            string respuesta = "";
            if (UsuarioBuscar == null)
            {
                respuesta = "Usuario No existe";
                HttpContext.Session.SetString("RespuestaAgregar", respuesta);
                return RedirectToAction("Solicitudes");
            }
            else
            {
                if (UsuarioBuscar.Usuario == UsuarioActivo.Usuario)
                {
                    respuesta = "No puedes agregarte a ti mismo";
                    HttpContext.Session.SetString("RespuestaAgregar", respuesta);
                    return RedirectToAction("Solicitudes");
                }
                Contacto temp = new Contacto();
                temp.Usuario = UsuarioActivo.Usuario;
                temp.Usuario2 = UsuarioBuscar.Usuario;
                HttpResponseMessage response3 = RutaApi.Api.PostAsJsonAsync("Usuarios/BusquedaContacto", temp).Result;
                Contacto Contacto = response3.Content.ReadAsAsync<Contacto>().Result;
                Contacto Actualizar = new Contacto();
                Actualizar.Usuario = UsuarioActivo.Usuario;
                Actualizar.Usuario2 = UsuarioBuscar.Usuario;
                Actualizar.tipo = false;
                Actualizar.estado = false;
                if (Contacto != null)
                {
                    if (Contacto.estado == true)
                    {
                        respuesta = "Usuario ya esta agregado";
                        HttpContext.Session.SetString("RespuestaAgregar", respuesta);
                        return RedirectToAction("Solicitudes");
                    }
                    else
                    {
                        if (Contacto.Usuario == UsuarioActivo.Usuario)
                        {
                            respuesta = "Ya has mandado un solicitud a este usuario";
                            HttpContext.Session.SetString("RespuestaAgregar", respuesta);
                            return RedirectToAction("Solicitudes");
                        }

                        Actualizar.estado = true;
                        HttpResponseMessage response4 = RutaApi.Api.PostAsJsonAsync("Usuarios/ActualizarContacto", Actualizar).Result;
                    }
                }
                else
                {
                    HttpResponseMessage response4 = RutaApi.Api.PostAsJsonAsync("Usuarios/NuevoContacto", Actualizar).Result;
                }
                respuesta = "Solicitud enviado!";
                HttpContext.Session.SetString("RespuestaAgregar", respuesta);
                return RedirectToAction("Solicitudes");
            }
        }
        public ActionResult Aceptar(string valor)
        {
            Usuarios UsuarioActivo = UsuarioActivoFuncion();
            while (UsuarioActivo == null)
            {
                UsuarioActivo = UsuarioActivoFuncion();
            }
            //Usuarios UsuarioActivo = new Usuarios();
            //UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
            //HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
            //UsuarioActivo = response.Content.ReadAsAsync<Usuarios>().Result;

            Usuarios UsuarioAceptar = new Usuarios();
            UsuarioAceptar.Usuario = valor;
            HttpResponseMessage response2 = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioAceptar).Result;
            UsuarioAceptar = response2.Content.ReadAsAsync<Usuarios>().Result;
            if (UsuarioAceptar != null)
            {
                Contacto temp = new Contacto();
                temp.Usuario = UsuarioActivo.Usuario;
                temp.Usuario2 = UsuarioAceptar.Usuario;
                HttpResponseMessage response3 = RutaApi.Api.PostAsJsonAsync("Usuarios/BusquedaContacto", temp).Result;
                Contacto Contacto = response3.Content.ReadAsAsync<Contacto>().Result;
                if (Contacto.estado == true)
                {
                    return RedirectToAction("Solicitudes");
                }
                else
                {
                    Contacto Actualizar = new Contacto();
                    Actualizar.Usuario = UsuarioAceptar.Usuario;
                    Actualizar.Usuario2 = UsuarioActivo.Usuario;
                    Actualizar.tipo = false;
                    Actualizar.estado = true;
                    HttpResponseMessage response4 = RutaApi.Api.PostAsJsonAsync("Usuarios/ActualizarContacto", Actualizar).Result;
                }
            }
            else
            {
                return RedirectToAction("EntradaUsuarioActivo");
            }


            return RedirectToAction("Solicitudes");
        }
        public ActionResult Main()
        {
            HttpContext.Session.SetString("Usuario", "");
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind("Usuario,Contraseña")] Usuarios UsuarioInfo)
        {
            if (UsuarioInfo.Usuario == null || UsuarioInfo.Contraseña == null)
            {
                return RedirectToAction("Login");
            }
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Ingreso", UsuarioInfo).Result;
            var x = response.Content.ReadAsStringAsync().Result;
            if (x == "Exito" && HttpContext.Session.GetString("Usuario") == "")
            {
                HttpContext.Session.SetString("Usuario", UsuarioInfo.Usuario);
                HttpContext.Session.SetString("RespuestaAgregar", "");
                return RedirectToAction("EntradaUsuarioActivo", null);
            }
            return RedirectToAction("Login");
        }

        public IActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar([Bind("Usuario,Nombre,Apellido,Contraseña,edad,fecha")] Usuarios UsuarioInfo)
        {
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Nuevo", UsuarioInfo).Result;
            var x = response.Content.ReadAsStringAsync().Result;
            if (x == "Agregado con exito")
            {
                return View("Main");
            }
            else if (x == "Este Usuario ya existe")
            {
                return View("Registrar");
            }
            return RedirectToAction("Registrar");
        }
        public async Task<ActionResult> MandarMensaje(string texto, IFormFile file)
        {
            Usuarios UsuarioActivo = UsuarioActivoFuncion();
            while (UsuarioActivo == null)
            {
                UsuarioActivo = UsuarioActivoFuncion();
            }
            Mensajes MensajeNuevo = new Mensajes();
            if (texto == null || texto == "")
            {
                if (file == null)
                {
                    return RedirectToAction("EntradaUsuarioActivo", new { valor = HttpContext.Session.GetString("Mensajeu2") });
                }
            }
            else
            {
                MensajeNuevo.Texto = texto;
            }
            MensajeNuevo.UsuarioE = UsuarioActivo.Usuario;
            MensajeNuevo.publickey1 = UsuarioActivo.PublickKey;
            MensajeNuevo.Fecha = DateTime.Now;
            MensajeNuevo.Esconder = false;
            //ListaDeArchivos
            if (file != null)
            {
                HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Mensajes2/ListaMensajesTotal", MensajeNuevo).Result;
                var listaR = response.Content.ReadAsAsync<List<Mensajes>>().Result;
                string cantidad;
                if (listaR != null)
                {
                    cantidad = Convert.ToString(listaR.Count() + 1);
                }
                else
                {
                    cantidad = "1";
                }
                ArchivosValores archivo = new ArchivosValores();
                archivo.Nombre = cantidad + file.FileName;
                archivo.NombreOriginal = file.FileName;
                string ruta = Path.Combine(x.ContentRootPath, "Archivos", cantidad + file.FileName);
                using (var x = new FileStream(ruta, FileMode.Create))
                {
                    await file.CopyToAsync(x);
                }

                MensajeNuevo.FileString = cantidad + file.FileName;
                MensajeNuevo.FileStringOriginal = file.FileName;
                FileStream Archivobyte = System.IO.File.OpenRead(ruta);
                byte[] archivoinfo = new byte[Archivobyte.Length];
                Archivobyte.Read(archivoinfo, 0, archivoinfo.Length);
                archivo.archivo = archivoinfo;
                Archivobyte.Close();

                await RutaApi.Api.PostAsJsonAsync("Mensajes2/Comprimir", archivo);
            }


            if (HttpContext.Session.GetString("Mensajeu2") == "")
            {
                if (HttpContext.Session.GetString("NGrupo") != "")
                {
                    string nombre = HttpContext.Session.GetString("NGrupo");
                    int Num = Convert.ToInt32(HttpContext.Session.GetString("Num"));
                    Contacto Grupo = new Contacto();
                    Grupo.nombre = nombre;
                    Grupo.num = Num;
                    MensajeNuevo.GNombre = nombre;
                    MensajeNuevo.Num = Num;
                    HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/RetornarGrupo", Grupo).Result;
                    Contacto GrupoValores = response.Content.ReadAsAsync<Contacto>().Result;
                    MensajeNuevo.Usuarios = GrupoValores.Usuarios;
                    await RutaApi.Api.PostAsJsonAsync("Mensajes2/MandarMensaje", MensajeNuevo);
                    return RedirectToAction("EntradaUsuarioActivo", new { nombre = Grupo.nombre, Num = Grupo.num });
                }
            }
            else
            {
                MensajeNuevo.UsuarioR = HttpContext.Session.GetString("Mensajeu2");
                await RutaApi.Api.PostAsJsonAsync("Mensajes2/MandarMensaje", MensajeNuevo);
                return RedirectToAction("EntradaUsuarioActivo", new { valor = MensajeNuevo.UsuarioR });
            }


            return RedirectToAction("EntradaUsuarioActivo");
        }
        public Usuarios UsuarioActivoFuncion()
        {
            Usuarios UsuarioActivo = new Usuarios();
            UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
            UsuarioActivo = response.Content.ReadAsAsync<Usuarios>().Result;
            return UsuarioActivo;
        }
        public ActionResult EliminarMensaje(string Usuario, string Usuario2, string nombre, int num, string texto, DateTime fecha, bool Paratodos)
        {

            Usuarios UsuarioActivo = UsuarioActivoFuncion();
            Mensajes temp = new Mensajes();
            temp.UsuarioE = Usuario;
            temp.UsuarioR = Usuario2;
            temp.GNombre = nombre;
            temp.Num = num;
            temp.Texto = texto;
            temp.Fecha = fecha;
            if (nombre == null)
            {
                nombre = "";
            }
            if (Paratodos == true)
            {
                HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Mensajes2/EliminarTodos", temp).Result;
            }
            else
            {
                HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Mensajes2/EliminarSolo", temp).Result;
            }
            return RedirectToAction("EntradaUsuarioActivo", new { nombre = nombre, Num = num, valor = Usuario2 });

        }

        public ActionResult CrearGrupo2(string valor)
        {
            if (valor == null)
            {
                return RedirectToAction("EntradaUsuarioActivo");
            }
            Usuarios UsuarioActivo = UsuarioActivoFuncion();
            while (UsuarioActivo == null)
            {
                UsuarioActivo = UsuarioActivoFuncion();
            }
            Contacto Newgrupo = new Contacto();
            Newgrupo.nombre = valor;
            //Newgrupo.Usuario = UsuarioActivo.Usuario;
            Newgrupo.tipo = true;
            Newgrupo.estado = true;
            List<Contacto> Temp = new List<Contacto>();
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/ContactoGrupo", Newgrupo).Result;
            List<Contacto> listacontactos = response.Content.ReadAsAsync<List<Contacto>>().Result;
            Newgrupo.num = listacontactos.Count() + 1;
            Newgrupo.Usuarios.Add(UsuarioActivo.Usuario);
            RutaApi.Api.PostAsJsonAsync("Usuarios/NuevoContacto", Newgrupo);

            return RedirectToAction("EntradaUsuarioActivo");
        }
        public ActionResult AgregarAlGrupo(string UsuarioAgregar)
        {
            string nombre = HttpContext.Session.GetString("NGrupo");
            int Num = Convert.ToInt32(HttpContext.Session.GetString("Num"));
            string respuesta = "";
            if (UsuarioAgregar == null || UsuarioAgregar == "")
            {
                respuesta = "Ingrese un usuario primero";
            }
            else
            {
                Usuarios UsuarioBuscar = new Usuarios();
                UsuarioBuscar.Usuario = UsuarioAgregar;
                HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioBuscar).Result;
                UsuarioBuscar = response.Content.ReadAsAsync<Usuarios>().Result;
                if (UsuarioBuscar == null)
                {
                    respuesta = "Usuario No existe";
                }
                else
                {
                    response = RutaApi.Api.PostAsJsonAsync("Usuarios/BuscuarGruposUsuario", UsuarioBuscar).Result;
                    List<Contacto> Contacto = response.Content.ReadAsAsync<List<Contacto>>().Result;
                    foreach (var k in Contacto)
                    {
                        if (k.nombre == nombre && k.num == Num)
                        {
                            respuesta = "Usuario ya pertenece al grupo";
                            return RedirectToAction("EntradaUsuarioActivo", new { RespuestaAgregarGrupo = respuesta, nombre = nombre, Num = Num });
                        }
                    }
                    Contacto temp = new Contacto();
                    temp.num = Num;
                    temp.nombre = nombre;
                    response = RutaApi.Api.PostAsJsonAsync("Usuarios/RetornarGrupo", temp).Result;
                    temp = response.Content.ReadAsAsync<Contacto>().Result;
                    temp.Usuarios.Add(UsuarioBuscar.Usuario);
                    RutaApi.Api.PostAsJsonAsync("Usuarios/ActualizarContactoGrupo", temp);
                    respuesta = "Usuario agregado al grupo!";
                }
            }

            return RedirectToAction("EntradaUsuarioActivo", new { RespuestaAgregarGrupo = respuesta, nombre = nombre, Num = Num });
        }
        public ActionResult DescargarArchivos(string Usuario, string Usuario2, string nombre, int num, string NombreArchivo, string NombreOriginal)
        {
            if (F.verificar == false)
            {
                F.verificar = true;
                ArchivosValores ArchivoDescargar = new ArchivosValores();
                ArchivoDescargar.Nombre = NombreArchivo;
                ArchivoDescargar.NombreOriginal = NombreOriginal;
                HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Mensajes2/Descomprimir", ArchivoDescargar).Result;
                ArchivoDescargar = response.Content.ReadAsAsync<ArchivosValores>().Result;
                F.verificar = false;
                return File(ArchivoDescargar.archivo, System.Net.Mime.MediaTypeNames.Application.Octet, NombreOriginal);
            }
            else
            {
                return Ok();
            }

        }
        public ActionResult BuscarMensaje(string mensaje)
        {
            string nombre = HttpContext.Session.GetString("NGrupo");
            if (nombre != "")
            {
                int Num = Convert.ToInt32(HttpContext.Session.GetString("Num"));
                return RedirectToAction("EntradaUsuarioActivo", new { VerficiarTextofiltro = mensaje, nombre = nombre, Num = Num });
            }
            else
            {
                string Usuario2 = HttpContext.Session.GetString("Mensajeu2");
                return RedirectToAction("EntradaUsuarioActivo", new { VerficiarTextofiltro = mensaje, valor = Usuario2 });
            }
        }






    }
}
