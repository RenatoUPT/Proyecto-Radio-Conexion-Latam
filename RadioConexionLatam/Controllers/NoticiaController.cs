using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadioConexionLatam.Controllers
{
    public class NoticiaController : Controller
    {
        // GET: Noticia
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CrearNoticia()
        {
            return View();
        }

        public ActionResult VisualizarNoticia()
        {
            return View();
        }

    }
}