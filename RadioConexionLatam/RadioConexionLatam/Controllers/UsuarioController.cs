using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RadioConexionLatam.Models;

namespace RadioConexionLatam.Controllers
{
    public class UsuarioController : Controller
    {
        // Contexto de la base de datos (Asumiendo que usas Entity Framework)
        private Model1 db = new Model1();

        // GET: Usuario
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        // GET: Usuario/CrearUsuario
        public ActionResult CrearUsuario()
        {
            ViewBag.Roles = db.Roles.Select(r => new SelectListItem
            {
                Value = r.idRol.ToString(),
                Text = r.descripcion
            }).ToList();
            return View();
        }

        // POST: Usuario/CrearUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearUsuario([Bind(Include = "nombre,apellido,correo,contrasena,idRol")] Usuarios usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Error al intentar guardar el usuario. Error: " + ex.Message);
            }

            return View(usuario);
        }

        // GET: Usuario/VisualizarUsuario
        public ActionResult VisualizarUsuario(int id)
        {
            Usuarios usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }
    }
}
