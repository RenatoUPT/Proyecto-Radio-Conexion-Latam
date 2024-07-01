using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadioConexionLatam.Controllers
{
    public class AnuncioController : Controller
    {
        // GET: Anuncio
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CrearAnuncio()
        {
            return View();
        }

        public ActionResult VisualizarAnuncio()
        {
            return View();
        }


    }
}