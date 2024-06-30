using RadioConexionLatam.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Net;

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
        public ActionResult CrearAnuncio(Anuncios anuncio, HttpPostedFileBase ImagenFile, IEnumerable<HttpPostedFileBase> imageFiles, string VideoUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre");
                return View(anuncio);
            }

            using (var db = new Model1())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Process the main image
                        if (ImagenFile != null && ImagenFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(ImagenFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/images/Anuncios"), Guid.NewGuid().ToString() + "_" + fileName);  // Use GUID to prevent name conflicts
                            ImagenFile.SaveAs(path);
                            anuncio.idImagenPrincipal = SaveImageToDatabase(db, fileName, path, "Main image for " + anuncio.titulo);
                        }

                        // Create carousel if there are images
                        if (imageFiles != null && imageFiles.Any())
                        {
                            int? idCarrusel = CreateCarruselWithImages(db, imageFiles, "Carousel for " + anuncio.titulo);
                            anuncio.idCarrusel = idCarrusel; // Asignar el idCarrusel al anuncio
                        }

                        // Process video if provided
                        if (!string.IsNullOrEmpty(VideoUrl))
                        {
                            anuncio.idVideoPrincipal = SaveVideoToDatabase(db, VideoUrl);
                        }

                        anuncio.fechaPublicacion = DateTime.Now;
                        db.Anuncios.Add(anuncio);
                        db.SaveChanges();
                        dbContextTransaction.Commit(); // Confirmar la transacción si todo está bien
                        return RedirectToAction("VisualizarAnuncio");
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        dbContextTransaction.Rollback();

                        // Captura el mensaje de error completo, incluyendo las excepciones internas
                        var error = ex.InnerException?.InnerException?.Message ?? ex.Message;
                        ViewBag.ErrorMessage = "Error al guardar el anuncio: " + error;
                        ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre");

                        return View(anuncio);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback(); // Rollback transaction on error
                        ViewBag.ErrorMessage = "Error general al guardar el anuncio: " + ex.Message;
                        ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre");
                        return View(anuncio);
                    }
                }
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

            using (var db = new Model1())
            {
                var anuncioExistente = db.Anuncios
                    .Include(a => a.Carrusel)
                    .Include(a => a.Carrusel.DetalleCarrusel)
                    .Include(a => a.Imagenes)
                    .Include(a => a.Videos)
                    .FirstOrDefault(a => a.idAnuncio == anuncio.idAnuncio);

                if (anuncioExistente == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    // Update basic fields
                    anuncioExistente.titulo = anuncio.titulo;
                    anuncioExistente.subtitulo = anuncio.subtitulo;
                    anuncioExistente.contenido = anuncio.contenido;
                    anuncioExistente.fechaPublicacion = DateTime.Now;
                    anuncioExistente.idCategoria = anuncio.idCategoria;
                    anuncioExistente.estado = anuncio.estado;

                    // Handle main image
                    if (SinImagen && anuncioExistente.idImagenPrincipal.HasValue)
                    {
                        var imagen = db.Imagenes.Find(anuncioExistente.idImagenPrincipal.Value);
                        db.Imagenes.Remove(imagen);
                        anuncioExistente.idImagenPrincipal = null;
                    }
                    else if (ImagenFile != null && ImagenFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(ImagenFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/images/Anuncios"), fileName);
                        ImagenFile.SaveAs(path);
                        var imageId = SaveImageToDatabase(db, fileName, path, "Updated main image for " + anuncio.titulo);
                        anuncioExistente.idImagenPrincipal = imageId;
                    }

                    // Handle video
                    if (SinVideo && anuncioExistente.idVideoPrincipal.HasValue)
                    {
                        var video = db.Videos.Find(anuncioExistente.idVideoPrincipal.Value);
                        db.Videos.Remove(video);
                        anuncioExistente.idVideoPrincipal = null;
                    }
                    else if (!string.IsNullOrEmpty(VideoUrl))
                    {
                        var videoId = SaveVideoToDatabase(db, VideoUrl);
                        anuncioExistente.idVideoPrincipal = videoId;
                    }

                    // Handle carousel images
                    if (imageFiles != null && imageFiles.Any())
                    {
                        if (anuncioExistente.idCarrusel.HasValue)
                        {
                            var existingCarousel = db.Carrusel.Include(c => c.DetalleCarrusel).FirstOrDefault(c => c.idCarrusel == anuncioExistente.idCarrusel.Value);
                            if (existingCarousel != null)
                            {
                                db.DetalleCarrusel.RemoveRange(existingCarousel.DetalleCarrusel);
                            }
                        }
                        int? idCarrusel = CreateCarruselWithImages(db, imageFiles, "Updated carousel for " + anuncio.titulo);
                        anuncioExistente.idCarrusel = idCarrusel;
                    }

                    db.SaveChanges();
                    return RedirectToAction("VisualizarAnuncio");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error updating the announcement: " + ex.Message;
                    ViewBag.Categorias = new SelectList(GetCategorias(), "idCategoria", "nombre", anuncio.idCategoria);
                    return View(anuncio);
                }
            }
        }

        private int SaveImageToDatabase(Model1 db, string fileName, string filePath, string description)
        {
            var image = new Imagenes { url = "/Content/images/Anuncios/" + fileName, descripcion = description };
            db.Imagenes.Add(image);
            db.SaveChanges();
            return image.idImagen;
        }

        private int SaveVideoToDatabase(Model1 db, string videoUrl)
        {
            var video = new Videos { url = videoUrl, descripcion = "Video from " + videoUrl };
            db.Videos.Add(video);
            db.SaveChanges();
            return video.idVideo;
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

        private int? CreateCarruselWithImages(Model1 db, IEnumerable<HttpPostedFileBase> imageFiles, string description)
        {
            if (!imageFiles.Any())
            {
                return null; // No files to process
            }

            var carrusel = new Carrusel { descripcion = description };
            db.Carrusel.Add(carrusel);
            db.SaveChanges(); // Save to get ID

            foreach (var file in imageFiles)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/Carrusel"), fileName);
                    try
                    {
                        file.SaveAs(path); // Save the file physically

                        var detalleCarrusel = new DetalleCarrusel
                        {
                            url = "/Content/images/Carrusel/" + fileName, // Save the path in DetalleCarrusel table
                            idCarrusel = carrusel.idCarrusel // Link the detail to the carousel
                        };
                        db.DetalleCarrusel.Add(detalleCarrusel);
                    }
                    catch (Exception ex)
                    {
                        // Log the error (consider using a logging framework or tool)
                        Debug.WriteLine("Error saving file: " + ex.Message);
                    }
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                // Log the error or examine the exception object while debugging
                var innerException = ex.InnerException?.InnerException;
                if (innerException != null)
                {
                    // Log inner exception details, which often include the actual database error
                    Console.WriteLine(innerException.Message);
                }
                throw; // Re-throw the exception for further handling or logging outside this block
            }
            return carrusel.idCarrusel;
        }


    }
}