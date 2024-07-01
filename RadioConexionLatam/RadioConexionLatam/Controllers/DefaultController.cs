using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadioConexionLatam.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        //vista de publicitaria
        public ActionResult Index()
        {
            return View();
        }
        //vista de crud donde estara la funcionalidad y se reflejara a la vista publicitaria
        

    }
}