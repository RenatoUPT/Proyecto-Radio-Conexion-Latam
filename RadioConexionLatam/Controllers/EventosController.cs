using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RadioConexionLatam.Models;

namespace RadioConexionLatam.Controllers
{
    public class EventosController : Controller
    {
        private Model1 db = new Model1();

        // GET: Eventos
        public ActionResult Index()
        {
            var eventos = db.Eventos.Include(e => e.Categoria).ToList();
            return View(eventos);
        }

        // GET: Eventos/DetallesEvento/5
        public ActionResult DetallesEvento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // GET: Eventos/CrearEvento
        public ActionResult CrearEvento()
        {
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre");
            return View();
        }

        // POST: Eventos/CrearEvento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearEvento([Bind(Include = "idEvento,nombreEvento,descripcion,fechaEvento,lugar,organizador,estado,capacidad,idCategoria")] Eventos evento, HttpPostedFileBase ImagenFile)
        {
            if (ModelState.IsValid)
            {
                if (ImagenFile != null && ImagenFile.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(ImagenFile.InputStream))
                    {
                        evento.Imagen = reader.ReadBytes(ImagenFile.ContentLength);
                    }
                }

                db.Eventos.Add(evento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", evento.idCategoria);
            return View(evento);
        }

        // GET: Eventos/EditarEvento/5
        public ActionResult EditarEvento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", evento.idCategoria);
            return View(evento);
        }

        // POST: Eventos/EditarEvento/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEvento([Bind(Include = "idEvento,nombreEvento,descripcion,fechaEvento,lugar,organizador,estado,capacidad,idCategoria,Imagen")] Eventos evento, HttpPostedFileBase ImagenFile)
        {
            if (ModelState.IsValid)
            {
                if (ImagenFile != null && ImagenFile.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(ImagenFile.InputStream))
                    {
                        evento.Imagen = reader.ReadBytes(ImagenFile.ContentLength);
                    }
                }

                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", evento.idCategoria);
            return View(evento);
        }

        // GET: Eventos/EliminarEvento/5
        public ActionResult EliminarEvento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Eventos/EliminarEvento/5
        [HttpPost, ActionName("EliminarEvento")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEventoConfirmado(int id)
        {
            Eventos evento = db.Eventos.Find(id);
            db.Eventos.Remove(evento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VistaPublicitaria()
        {
            var eventosActivos = db.Eventos.Include(e => e.Categoria)
                                            .Where(e => e.estado == "A")
                                            .ToList();
            return View(eventosActivos);
        }

    }
}