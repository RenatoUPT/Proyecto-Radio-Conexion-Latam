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

        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        public ActionResult CrearUsuario()
        {
            ViewBag.Roles = db.Roles.Select(r => new SelectListItem
            {
                Value = r.idRol.ToString(),
                Text = r.descripcion
            }).ToList();
            ViewBag.estado = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Activo", Value = "A" },
                new SelectListItem { Text = "Inactivo", Value = "I" }
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearUsuario([Bind(Include = "nombre,apellido,correo,contrasena,idRol,estado")] Usuarios usuario)
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

            ViewBag.Roles = db.Roles.Select(r => new SelectListItem
            {
                Value = r.idRol.ToString(),
                Text = r.descripcion
            }).ToList();
            ViewBag.estado = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Activo", Value = "A" },
                new SelectListItem { Text = "Inactivo", Value = "I" }
            }, "Value", "Text");

            return View(usuario);
        }

        public ActionResult VisualizarUsuario()
        {
            var modelo = db.Usuarios.ToList();
            return View(modelo);
        }

        public ActionResult EditarUsuario(int id)
        {
            var usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idRol = new SelectList(db.Roles, "idRol", "descripcion", usuario.idRol);
            ViewBag.estado = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Activo", Value = "A" },
                new SelectListItem { Text = "Inactivo", Value = "I" }
            }, "Value", "Text", usuario.estado);

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario([Bind(Include = "idUsuario,nombre,apellido,correo,contrasena,idRol,estado")] Usuarios usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("VisualizarUsuario");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Otro usuario ha modificado estos datos. Por favor, recargue la página y vuelva a intentarlo.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al intentar guardar los cambios: " + ex.Message);
            }

            ViewBag.idRol = new SelectList(db.Roles, "idRol", "descripcion", usuario.idRol);
            return View(usuario);
        }
    }
}
