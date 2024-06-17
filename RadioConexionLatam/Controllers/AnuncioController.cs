using RadioConexionLatam.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadioConexionLatam.Controllers
{
    public class AnuncioController : Controller
    {
        private Anuncios objAnuncios = new Anuncios();

        // GET: Anuncio
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CrearAnuncio()
        {
            ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CrearAnuncio(Anuncios anuncio, IEnumerable<HttpPostedFileBase> ImagenFiles, string VideoUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new Model1())
                    {
                        foreach (var file in ImagenFiles)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Content/images/Anuncios"), fileName);
                                file.SaveAs(path);
                                var imagenId = SaveImageToDatabase(fileName, path);

                                if (anuncio.idImagenPrincipal == null)
                                {
                                    anuncio.idImagenPrincipal = imagenId;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(VideoUrl))
                        {
                            anuncio.idVideoPrincipal = SaveVideoToDatabase(VideoUrl);
                        }

                        anuncio.fechaPublicacion = DateTime.Now;

                        db.Anuncios.Add(anuncio);
                        db.SaveChanges();

                        return RedirectToAction("VisualizarAnuncio");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al crear el anuncio: " + ex.Message);
            }

            ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre");
            return View(anuncio);
        }



        public ActionResult VisualizarAnuncio(string Buscar)
        {
            List<Anuncios> anuncios;
            using (var db = new Model1())
            {
                if (string.IsNullOrEmpty(Buscar))
                {
                    anuncios = db.Anuncios.ToList();
                }
                else
                {
                    anuncios = db.Anuncios
                                 .Where(x => x.idAnuncio.ToString() == Buscar || x.titulo.Contains(Buscar))
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

                    if (anuncio.idVideoPrincipal.HasValue)
                    {
                        anuncio.Videos = db.Videos.Find(anuncio.idVideoPrincipal.Value);
                        if (anuncio.Videos != null)
                        {
                            anuncio.VideoUrl = anuncio.Videos.url;
                        }
                    }
                }
            }
            return View(anuncios);
        }

        public ActionResult EditarAnuncio(int id)
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

            ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre", anuncio.idCategoria);
            return View(anuncio);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditarAnuncio(Anuncios anuncio, HttpPostedFileBase ImagenFile, string VideoUrl, bool SinImagen = false, bool SinVideo = false)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Model1())
                {
                    var anuncioExistente = db.Anuncios.Find(anuncio.idAnuncio);
                    if (anuncioExistente == null)
                    {
                        return HttpNotFound();
                    }

                    anuncioExistente.titulo = anuncio.titulo;
                    anuncioExistente.subtitulo = anuncio.subtitulo;
                    anuncioExistente.contenido = anuncio.contenido;
                    anuncioExistente.idCategoria = anuncio.idCategoria;

                    anuncioExistente.estado = anuncio.estado ?? "A";

                    // Procesar imagen
                    if (SinImagen)
                    {
                        anuncioExistente.idImagenPrincipal = null;
                    }
                    else if (ImagenFile != null && ImagenFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(ImagenFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/images/Anuncios"), fileName);
                        ImagenFile.SaveAs(path);
                        anuncioExistente.idImagenPrincipal = SaveImageToDatabase(fileName, path);
                    }

                    // Procesar video
                    if (SinVideo)
                    {
                        anuncioExistente.idVideoPrincipal = null;
                    }
                    else if (!string.IsNullOrEmpty(VideoUrl))
                    {
                        anuncioExistente.idVideoPrincipal = SaveVideoToDatabase(VideoUrl);
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("VisualizarAnuncio");
            }

            ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre", anuncio.idCategoria);
            return View(anuncio);
        }

        private int SaveImageToDatabase(string fileName, string filePath)
        {
            using (var db = new Model1())
            {
                var image = new Imagenes { url = "/Content/images/Anuncios/" + fileName, descripcion = fileName };
                db.Imagenes.Add(image);
                db.SaveChanges();
                return image.idImagen;
            }
        }

        private int SaveVideoToDatabase(string videoUrl)
        {
            using (var db = new Model1())
            {
                var video = new Videos { url = videoUrl, descripcion = "Video from " + videoUrl };
                db.Videos.Add(video);
                db.SaveChanges();
                return video.idVideo;
            }
        }

        private List<Categorias> GetCategorias()
        {
            using (var db = new Model1())
            {
                return db.Categorias.ToList();
            }
        }

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


    }
}