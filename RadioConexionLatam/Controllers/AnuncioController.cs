using RadioConexionLatam.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
        public ActionResult CrearAnuncio(Anuncios anuncio, HttpPostedFileBase ImagenFile, IEnumerable<HttpPostedFileBase> imageFiles, string VideoUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(db.Categorias, "idCategoria", "nombre");
                return View(anuncio);
            }

            try
            {
                if (ImagenFile != null && ImagenFile.ContentLength > 0)
                {
                    anuncio.idImagenPrincipal = SaveImageToDatabase(ImagenFile, "~/Content/images/Anuncios", "Main image for " + anuncio.titulo);
                }

                if (!string.IsNullOrEmpty(VideoUrl))
                {
                    anuncio.idVideoPrincipal = SaveVideoToDatabase(VideoUrl);
                }

                if (imageFiles != null && imageFiles.Any())
                {
                    anuncio.idCarrusel = CreateCarruselWithImages(imageFiles, "Carousel for " + anuncio.titulo);
                }

                anuncio.fechaPublicacion = DateTime.Now;
                db.Anuncios.Add(anuncio);
                db.SaveChanges();
                return RedirectToAction("VisualizarAnuncio");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al guardar el anuncio: " + ex.Message;
                ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre", anuncio.idCategoria);
                return View(anuncio);
            }
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
            try
            {
                using (var db = new Model1())
                {
                    anuncio = db.Anuncios
                                .Include(a => a.Carrusel.DetalleCarrusel)
                                .Include(a => a.Imagenes)
                                .Include(a => a.Videos)
                                .FirstOrDefault(a => a.idAnuncio == id);

                    if (anuncio == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre", anuncio.idCategoria);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error retrieving ad for editing: " + ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error accessing the data.");
            }

            return View(anuncio);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditarAnuncio(Anuncios anuncio, HttpPostedFileBase ImagenFile, IEnumerable<HttpPostedFileBase> imageFiles, string VideoUrl, bool SinImagen = false, bool SinVideo = false)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre", anuncio.idCategoria);
                return View(anuncio);
            }

            var anuncioExistente = db.Anuncios.Find(anuncio.idAnuncio);
            if (anuncioExistente == null)
            {
                return HttpNotFound();
            }

            anuncioExistente.titulo = anuncio.titulo;
            anuncioExistente.subtitulo = anuncio.subtitulo;
            anuncioExistente.contenido = anuncio.contenido;
            anuncioExistente.estado = anuncio.estado ?? "A";
            anuncioExistente.idCategoria = anuncio.idCategoria;

            ProcesarMultimedia(anuncioExistente, ImagenFile, VideoUrl, SinImagen, SinVideo);

            // Manejo del carrusel de imágenes
            if (imageFiles != null && imageFiles.Any())
            {
                if (anuncioExistente.idCarrusel.HasValue)
                {
                    // Elimina el carrusel existente si hay uno
                    RemoveCarrusel(anuncioExistente.idCarrusel.Value);
                }
                // Crea un nuevo carrusel
                anuncioExistente.idCarrusel = CreateCarruselWithImages(imageFiles, "Updated Carousel for " + anuncio.titulo);
            }

            db.SaveChanges();
            return RedirectToAction("VisualizarAnuncio");
        }

        private void RemoveCarrusel(int carruselId)
        {
            var existingCarrusel = db.Carrusel.Include(c => c.DetalleCarrusel).FirstOrDefault(c => c.idCarrusel == carruselId);
            if (existingCarrusel != null)
            {
                db.DetalleCarrusel.RemoveRange(existingCarrusel.DetalleCarrusel);
                db.Carrusel.Remove(existingCarrusel);
                db.SaveChanges();
            }
        }

        private int? CreateCarruselWithImages(IEnumerable<HttpPostedFileBase> imageFiles, string description)
        {
            // Asegurarse de que la descripción no exceda 100 caracteres
            if (description.Length > 100)
            {
                description = description.Substring(0, 100); // Trunca a 100 caracteres
            }

            var carrusel = new Carrusel { descripcion = description };
            db.Carrusel.Add(carrusel);
            db.SaveChanges();  // Guarda para obtener el ID del carrusel

            foreach (var file in imageFiles)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/Carrusel"), fileName);
                    file.SaveAs(path);
                    var detalleCarrusel = new DetalleCarrusel { url = "/Content/images/Carrusel/" + fileName, idCarrusel = carrusel.idCarrusel };
                    db.DetalleCarrusel.Add(detalleCarrusel);
                }
            }

            db.SaveChanges();
            return carrusel.idCarrusel;
        }

        private void ProcesarMultimedia(Anuncios anuncioExistente, HttpPostedFileBase ImagenFile, string VideoUrl, bool SinImagen, bool SinVideo)
        {
            if (SinImagen && anuncioExistente.idImagenPrincipal.HasValue)
            {
                RemoveImage(anuncioExistente.idImagenPrincipal.Value);
                anuncioExistente.idImagenPrincipal = null;
            }
            else if (ImagenFile != null && ImagenFile.ContentLength > 0)
            {
                anuncioExistente.idImagenPrincipal = SaveImageToDatabase(ImagenFile, "~/Content/images/Anuncios", "Updated image for " + anuncioExistente.titulo);
            }

            if (SinVideo && anuncioExistente.idVideoPrincipal.HasValue)
            {
                RemoveVideo(anuncioExistente.idVideoPrincipal.Value);
                anuncioExistente.idVideoPrincipal = null;
            }
            else if (!string.IsNullOrEmpty(VideoUrl))
            {
                anuncioExistente.idVideoPrincipal = SaveVideoToDatabase(VideoUrl);
            }
        }

        private void RemoveImage(int imageId)
        {
            var image = db.Imagenes.Find(imageId);
            if (image != null)
            {
                db.Imagenes.Remove(image);
                db.SaveChanges();
            }
        }

        private void RemoveVideo(int videoId)
        {
            var video = db.Videos.Find(videoId);
            if (video != null)
            {
                db.Videos.Remove(video);
                db.SaveChanges();
            }
        }

        private int? SaveImageToDatabase(HttpPostedFileBase file, string path, string description)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath(path), fileName);
                file.SaveAs(filePath);

                var image = new Imagenes
                {
                    url = "/Content/images/Anuncios/" + fileName,
                    descripcion = description
                };

                db.Imagenes.Add(image);
                db.SaveChanges();
                return image.idImagen;
            }
            return null;
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
                anuncio = db.Anuncios
                    .Include(a => a.Imagenes)
                    .Include(a => a.Videos)
                    .Include(a => a.Carrusel.DetalleCarrusel)  // Include carousel details
                    .FirstOrDefault(a => a.idAnuncio == id);
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
                if (anuncio.Carrusel != null)
                {
                    anuncio.Carrusel.DetalleCarrusel = anuncio.Carrusel.DetalleCarrusel.ToList();
                }
            }
            return View(anuncio);
        }


        private int? CreateOrUpdateCarruselWithImages(IEnumerable<HttpPostedFileBase> imageFiles, string description, int? existingCarruselId = null)
        {
            Carrusel carrusel;

            if (existingCarruselId.HasValue)
            {
                carrusel = db.Carrusel.Include(c => c.DetalleCarrusel).FirstOrDefault(c => c.idCarrusel == existingCarruselId.Value);
                if (carrusel != null)
                {
                    db.DetalleCarrusel.RemoveRange(carrusel.DetalleCarrusel);
                }
            }
            else
            {
                carrusel = new Carrusel { descripcion = description };
                db.Carrusel.Add(carrusel);
            }

            db.SaveChanges(); // Save to get or confirm the ID

            foreach (var file in imageFiles)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images/Carrusel"), fileName);
                file.SaveAs(path);
                var detalleCarrusel = new DetalleCarrusel { url = "/Content/images/Carrusel/" + fileName, idCarrusel = carrusel.idCarrusel };
                db.DetalleCarrusel.Add(detalleCarrusel);
            }

            db.SaveChanges();
            return carrusel.idCarrusel;
        }

        private void UpdateAnuncio(Anuncios existingAnuncio, Anuncios newAnuncio, HttpPostedFileBase ImagenFile, string VideoUrl, bool removeImage, bool removeVideo)
        {
            existingAnuncio.titulo = newAnuncio.titulo;
            existingAnuncio.subtitulo = newAnuncio.subtitulo;
            existingAnuncio.contenido = newAnuncio.contenido;
            existingAnuncio.idCategoria = newAnuncio.idCategoria;
            existingAnuncio.estado = newAnuncio.estado ?? "A";

            if (removeImage)
            {
                existingAnuncio.idImagenPrincipal = null;
            }
            else if (ImagenFile != null && ImagenFile.ContentLength > 0)
            {
                existingAnuncio.idImagenPrincipal = SaveImageToDatabase(ImagenFile, "~/Content/images/Anuncios", "Updated image for " + existingAnuncio.titulo);
            }

            if (removeVideo)
            {
                existingAnuncio.idVideoPrincipal = null;
            }
            else if (!string.IsNullOrEmpty(VideoUrl))
            {
                existingAnuncio.idVideoPrincipal = SaveVideoToDatabase(VideoUrl);
            }
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
