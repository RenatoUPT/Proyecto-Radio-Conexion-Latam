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
                ModelState.AddModelError("", "Error al intentar guardar el usuario: " + ex.Message);
            }

            return View(usuario);
        }

        public ActionResult VisualizarUsuario()
        {
            var modelo = db.Usuarios.ToList(); 
            return View(modelo);
        }

        // GET: Usuario/EditarUsuario/5
        public ActionResult EditarUsuario(int id)
        {
            Usuarios usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idRol = new SelectList(db.Roles, "idRol", "descripcion", usuario.idRol);
            return View(usuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario([Bind(Include = "idUsuario,nombre,apellido,correo,idRol,contrasena,estado")] Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("VisualizarUsuario");
                }
                catch (DbUpdateException ex)
                {
                    // Log the error or show a message to the user
                    ModelState.AddModelError("", "No se pudo actualizar el usuario: " + ex.Message);
                }
            }

            ViewBag.idRol = new SelectList(db.Roles, "idRol", "descripcion", usuario.idRol);
            return View(usuario);
        }



    }
}
