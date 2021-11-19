using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Models;
using MVC.Clases;
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
        public ActionResult EntradaUsuarioActivo()
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


            //UsuarioActivo.Contactos.Add(UsuarioActivo);
            //Chats prueba = new Chats();
            //prueba.Usuarios.Add(UsuarioActivo);
            //prueba.Nombre = "Grupo 1";
            //UsuarioActivo.Chats.Add(prueba);
            //UsuarioActivo.Chats.Add(prueba);
            //UsuarioActivo.Chats.Add(prueba);

            ViewBag.contactos = listacontactos;
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
                if (k.estado == false)
                {
                    listaespera.Add(k);
                }
            }
            ViewBag.Solicitudes = listaespera;
            return View("Solicitudes");
        }
        //public ActionResult SolicitudesAccept()
        //{
        //    Usuarios UsuarioActivo = new Usuarios();
        //    UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
        //    HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
        //}
        //public ActionResult SolicitudMandar()
        //{

        //}
        public ActionResult Aceptar(String Usuario)
        {

            return RedirectToAction("EntradaUsuarioActivo");
        }

        //public ActionResult Chat([Bind("Tiipo,Nombre,Usuarios")] Chats UsuarioInfo)
        //{
        //    Usuarios UsuarioActivo = new Usuarios();
        //    UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
        //    HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
        //    UsuarioActivo = response.Content.ReadAsAsync<Usuarios>().Result;
        //}
        //public ActionResult Busqueda(String valor)
        //{
        //    Usuarios UsuarioActivo = new Usuarios();
        //    UsuarioActivo.Usuario = HttpContext.Session.GetString("Usuario");
        //    HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Busqueda", UsuarioActivo).Result;
        //    UsuarioActivo = response.Content.ReadAsAsync<Usuarios>().Result;
        //    return View("Entrada");
        //}
        public ActionResult Main()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind("Usuario,Contraseña")] Usuarios UsuarioInfo)
        {
            HttpResponseMessage response = RutaApi.Api.PostAsJsonAsync("Usuarios/Ingreso", UsuarioInfo).Result;
            var x = response.Content.ReadAsStringAsync().Result;
            if (x == "Exito")
            {
                HttpContext.Session.SetString("Usuario", UsuarioInfo.Usuario);
                return RedirectToAction("EntradaUsuarioActivo");
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
                return View("");
            }
            return RedirectToAction("Registrar");
        }
        public ActionResult PruebaChat(string valor3)
        {
            if (ViewBag.p != null)
            {
                string prueba = ViewBag.p;
            }
            List<Contacto> valor = new List<Contacto>();
            Contacto temp = new Contacto();
            temp.nombre = "pruebaaa";
            valor.Add(temp);
            valor.Add(temp);
            valor.Add(temp);
            List<String> valor2 = new List<String>();
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            valor2.Add("PruebaMensajeeeeeeeeeeeeeeeeeeeeeee");
            ViewBag.Contactos = valor;
            ViewBag.Mensajes = valor2;
            return View("Pruebachat");
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
