using RadioConexionLatam.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RadioConexionLatam.Controllers
{
    public class AnuncioController : Controller
    {
        private Anuncios objAnuncios = new Anuncios();
        private Model1 db = new Model1();

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
        public ActionResult CrearAnuncio(Anuncios anuncio, HttpPostedFileBase ImagenFile, string VideoUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Procesar imagen
                    if (ImagenFile != null && ImagenFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(ImagenFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/images/Anuncios"), fileName);
                        ImagenFile.SaveAs(path);
                        anuncio.idImagenPrincipal = SaveImageToDatabase(fileName, path);
                    }

                    // Procesar video
                    if (!string.IsNullOrEmpty(VideoUrl))
                    {
                        anuncio.idVideoPrincipal = SaveVideoToDatabase(VideoUrl);
                    }

                    anuncio.fechaPublicacion = DateTime.Now;

                    anuncio.Guardar();
                    return RedirectToAction("VisualizarAnuncio");
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

        // CRUD de Categorias

        // Listar Categorias
        public ActionResult ListarCategorias()
        {
            var categorias = db.Categorias.ToList();
            return View(categorias);
        }

        // Crear Categoria
        public ActionResult CrearCategoria()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCategoria(Categorias categoria)
        {
            if (ModelState.IsValid)
            {
                db.Categorias.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("ListarCategorias");
            }
            return View(categoria);
        }

        // Editar Categoria
        public ActionResult EditarCategoria(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCategoria(int idCategoria, string nombre)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Model1())
                {
                    var categoria = db.Categorias.Find(idCategoria);
                    if (categoria == null)
                    {
                        return HttpNotFound();
                    }
                    categoria.nombre = nombre;
                    db.SaveChanges();
                }
                return RedirectToAction("ListarCategorias");
            }
            return View();
        }


    }
}
