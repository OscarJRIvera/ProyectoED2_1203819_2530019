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

namespace MVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MVCContext _context;
        private readonly Models.Data.Singleton F = Models.Data.Singleton.Instance;
        public UsuariosController(MVCContext context)
        {
            _context = context;
        }
        public ActionResult EntradaUsuarioActivo(string nombre,string valor)
        {
            Usuarios UsuarioActivo = new Usuarios();
            UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
            UsuarioActivo = response.Content.ReadAsAsync<Usuarios>().Result;

            HttpResponseMessage response2 = RutaApi.Api.PostAsJsonAsync("Usuarios/Contactos", UsuarioActivo).Result;
            List<Contacto> listacontactos = response2.Content.ReadAsAsync<List<Contacto>>().Result;

            List<Contacto> ListaCont = new List<Contacto>();
            foreach (var k in listacontactos)
            {
                if (k.estado == true)
                {
                    ListaCont.Add(k);
                }
            }
            List<Mensajes> mensajes = new List<Mensajes>();
            if (valor != null || nombre != null)
            {
                HttpResponseMessage response3 = RutaApi.Api.PostAsJsonAsync("Mensajes2/BuscarMensajes", valor).Result;
                List<Mensajes> listaMensajes = response3.Content.ReadAsAsync<List<Mensajes>>().Result;
                ViewBag.Mensajes = listaMensajes;
            }
            else
            {
                ViewBag.Mensajes = mensajes;
            }
            ViewBag.Contactos = ListaCont;
            ViewBag.Comprobar = UsuarioActivo.Usuario;
            
            return View("Entrada");
        }
        public ActionResult Solicitudes()
        {
            Usuarios UsuarioActivo = new Usuarios();
            UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;

            HttpResponseMessage response2 = RutaApi.Api.PostAsJsonAsync("Usuarios/Contactos", UsuarioActivo).Result;
            List<Contacto> listacontactos = response2.Content.ReadAsAsync<List<Contacto>>().Result;
            List<Contacto> listaespera = new List<Contacto>();
            foreach (var k in listacontactos)
            {
                if (k.estado == false && k.Usuario!=UsuarioActivo.Usuario)
                {
                    listaespera.Add(k);
                }
            }
            ViewBag.Comprueba = UsuarioActivo.Usuario;
            ViewBag.Solicitudes = listaespera;
            ViewBag.Mensaje = HttpContext.Session.GetString("RespuestaAgregar");
            HttpContext.Session.SetString("RespuestaAgregar","");
            return View("Solicitudes");
        }
        
        public ActionResult Agregar(string BuscarContacto)
        {
            Usuarios UsuarioActivo = new Usuarios();
            UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
            UsuarioActivo = response.Content.ReadAsAsync<Usuarios>().Result;

            Usuarios UsuarioBuscar = new Usuarios();
            UsuarioBuscar.Usuario = BuscarContacto;
            HttpResponseMessage response2 = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioBuscar).Result;
            UsuarioBuscar = response2.Content.ReadAsAsync<Usuarios>().Result;
            string respuesta = "";
            if (UsuarioBuscar== null)
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
                HttpResponseMessage response3 = RutaApi.Api.PostAsJsonAsync("Usuarios/BusquedaContacto",temp).Result;
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
            Usuarios UsuarioActivo = new Usuarios();
            UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
            UsuarioActivo = response.Content.ReadAsAsync<Usuarios>().Result;

            Usuarios UsuarioAceptar = new Usuarios();
            UsuarioAceptar.Usuario = valor;
            HttpResponseMessage response2 = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioAceptar).Result;
            UsuarioAceptar = response2.Content.ReadAsAsync<Usuarios>().Result;
            if (UsuarioAceptar != null) {
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
            if (x == "Exito" && HttpContext.Session.GetString("Usuario")=="")
            {
                HttpContext.Session.SetString("Usuario", UsuarioInfo.Usuario);
                HttpContext.Session.SetString("RespuestaAgregar", "");
                return RedirectToAction("EntradaUsuarioActivo",null);
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
            HttpResponseMessage response =  RutaApi.Api.PostAsJsonAsync("Usuarios/Nuevo", UsuarioInfo).Result;
            var x =  response.Content.ReadAsStringAsync().Result;
            if (x == "Agregado con exito")
            {
                return View("Main");
            }
            else if (x == "Este Usuario ya existe")
            {
                return View("");
            }
            return RedirectToAction("Registrar");
        }
       
       






        // GET: Usuarios
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Usuarios.ToListAsync());
        //}

        //// GET: Usuarios/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var usuarios = await _context.Usuarios
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (usuarios == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(usuarios);
        //}

        //// GET: Usuarios/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Usuarios/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Usuario,Nombre,Apellido,Contraseña")] Usuarios usuarios)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(usuarios);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(usuarios);
        //}

        //// GET: Usuarios/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var usuarios = await _context.Usuarios.FindAsync(id);
        //    if (usuarios == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(usuarios);
        //}

        //// POST: Usuarios/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,Nombre,Apellido,Contraseña")] Usuarios usuarios)
        //{
        //    if (id != usuarios.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(usuarios);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UsuariosExists(usuarios.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(usuarios);
        //}

        //// GET: Usuarios/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var usuarios = await _context.Usuarios
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (usuarios == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(usuarios);
        //}

        //// POST: Usuarios/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var usuarios = await _context.Usuarios.FindAsync(id);
        //    _context.Usuarios.Remove(usuarios);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UsuariosExists(int id)
        //{
        //    return _context.Usuarios.Any(e => e.Id == id);
        //}
    }
}
