using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using RadioConexionLatam.Models;

namespace RadioConexionLatam.Controllers
{
    public class ProgramacionController : Controller
    {
        private Model1 db = new Model1();

        // GET: Programacion
        public ActionResult Index()
        {
            var programas = db.ProgramacionSemanal.Include(p => p.Usuario).Include(p => p.Categoria).ToList();
            var horas = programas.Select(p => int.Parse(p.Hora)).Distinct().OrderBy(h => h).ToList();

            ViewBag.Horas = horas;

            return View(programas);
        }

        public ActionResult VisualizarProgramacion()
        {
            var programas = db.ProgramacionSemanal.Include(p => p.Usuario).Include(p => p.Categoria).ToList();
            return View(programas);
        }

        // GET: Programacion/Create
        // GET: Programacion/Create
        public ActionResult Crear()
        {
            ViewBag.idUsuario = new SelectList(db.Usuarios, "idUsuario", "nombre", "apellido");
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre");
            return View();
        }

        // POST: Programacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(ProgramacionSemanal programa)
        {
            if (ModelState.IsValid)
            {
                db.ProgramacionSemanal.Add(programa);
                db.SaveChanges();
                return RedirectToAction("VisualizarProgramacion");
            }

            ViewBag.idUsuario = new SelectList(db.Usuarios, "idUsuario", "nombre", "apellido", programa.idUsuario);
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", programa.idCategoria);
            return View(programa);
        }

        // GET: Programacion/Edit/5
        public ActionResult Editar(int id)
        {
            var programa = db.ProgramacionSemanal.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }

            ViewBag.Usuarios = new SelectList(db.Usuarios, "idUsuario", "nombre", programa.idUsuario);
            ViewBag.Categorias = new SelectList(db.Categorias, "idCategoria", "nombre", programa.idCategoria);

            return View(programa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(ProgramacionSemanal programa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programa).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("VisualizarProgramacion");
            }

            ViewBag.Usuarios = new SelectList(db.Usuarios, "idUsuario", "nombre", programa.idUsuario);
            ViewBag.Categorias = new SelectList(db.Categorias, "idCategoria", "nombre", programa.idCategoria);

            return View(programa);
        }


        // GET: Programacion/Delete/5
        public ActionResult Eliminar(int id)
        {
            var programa = db.ProgramacionSemanal.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        // POST: Programacion/Delete/5
        [HttpPost]
        public ActionResult EliminarConfirmar(int id)
        {
            var programa = db.ProgramacionSemanal.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            db.ProgramacionSemanal.Remove(programa);
            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }
    }
}
