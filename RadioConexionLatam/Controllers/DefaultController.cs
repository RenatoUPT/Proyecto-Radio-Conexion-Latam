using RadioConexionLatam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//FARLEY
namespace RadioConexionLatam.Controllers
{
    public class DefaultController : Controller
    {
        private Anuncios objAnuncios = new Anuncios();

        // GET: Default
        public ActionResult Index(string Buscar)
        {
            List<Anuncios> anuncios;
            using (var db = new Model1())
            {
                if (string.IsNullOrEmpty(Buscar))
                {
                    anuncios = db.Anuncios
                                 .Where(a => a.estado == "A")
                                 .OrderByDescending(a => a.fechaPublicacion)
                                 .ToList();
                }
                else
                {
                    anuncios = db.Anuncios
                                 .Where(x => (x.idAnuncio.ToString() == Buscar || x.titulo.Contains(Buscar)) && x.estado == "A")
                                 .OrderByDescending(a => a.fechaPublicacion)
                                 .ToList();
                }


                foreach (var anuncio in anuncios)
                {
                    if (anuncio.idImagenPrincipal.HasValue)
                    {
                        anuncio.Imagenes = db.Imagenes.Find(anuncio.idImagenPrincipal.Value);
                        if (anuncio.Imagenes != null)
                        {
                            anuncio.ImagenRuta = anuncio.Imagenes.url;
                        }
                    }
                }
            }
            return View(anuncios);
        }


        // Vista de detalles
        public ActionResult DetallesAnuncio(int id)
        {
            Anuncios anuncio;
            using (var db = new Model1())
            {
                anuncio = db.Anuncios.Find(id);
                if (anuncio == null)
                {
                    return HttpNotFound();
                }

                if (anuncio.idImagenPrincipal.HasValue)
                {
                    anuncio.Imagenes = db.Imagenes.Find(anuncio.idImagenPrincipal.Value);
                    if (anuncio.Imagenes != null)
                    {
                        anuncio.ImagenRuta = anuncio.Imagenes.url;
                    }
                }

                if (anuncio.idVideoPrincipal.HasValue)
                {
                    anuncio.Videos = db.Videos.Find(anuncio.idVideoPrincipal.Value);
                    if (anuncio.Videos != null)
                    {
                        anuncio.VideoUrl = anuncio.Videos.url;
                    }
                }
            }

            return View(anuncio);
        }



        public ActionResult Nosotros()
        {
                      
            return View();
        }
        public ActionResult Programacion()
        {

            return View();
        }

    }
}