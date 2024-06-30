using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
                    return RedirectToAction("VisualizarUsuario");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or show an error message
                ModelState.AddModelError("", "Error al intentar guardar el usuario: " + ex.Message);
            }

            return View(usuario);
        }

        public ActionResult VisualizarUsuario()
        {
            var modelo = db.Usuarios.ToList(); // Obtiene los usuarios de la base de datos y los almacena en 'modelo'
            return View(modelo);  // Pasa la lista de usuarios a la vista
        }

        public ActionResult EditarUsuario(int id)
        {
            var usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idRol = new SelectList(db.Roles, "idRol", "descripcion", usuario.idRol);
            return View(usuario);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario([Bind(Include = "idUsuario,nombre,apellido,correo,contrasena,idRol")] Usuarios usuario)
        {
            try
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("VisualizarUsuario");
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Otro usuario ha modificado estos datos. Por favor, recargue la página y vuelva a intentarlo.");
                return View(usuario);
            }
            ViewBag.idRol = new SelectList(db.Roles, "idRol", "descripcion", usuario.idRol);
            return View(usuario);
        }


    }
}
