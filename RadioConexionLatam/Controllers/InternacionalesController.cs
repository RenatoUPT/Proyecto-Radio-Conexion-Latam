using HtmlAgilityPack;
using RadioConexionLatam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RadioConexionLatam.Controllers
{
    public class InternacionalesController : Controller
    {
        private readonly Dictionary<string, string> categorias = new Dictionary<string, string>
        {
            { "Japon", "https://somoskudasai.com/categoria/noticias/japon/" },
            { "Anime", "https://somoskudasai.com/categoria/noticias/anime/" },
            { "Cultura Otaku", "https://somoskudasai.com/categoria/noticias/cultura-otaku/" },
            { "Manga", "https://somoskudasai.com/categoria/noticias/manga/" },
            { "Videojuegos", "https://somoskudasai.com/categoria/noticias/videojuegos/" }
        };

        // GET: Internacionales
        public async Task<ActionResult> Index(string categoria = "Japon", int page = 1, int pageSize = 10)
        {
            var noticias = await ObtenerNoticiasInternacionales(categoria);
            var paginatedNoticias = noticias.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Categorias = new SelectList(categorias.Keys);
            ViewBag.CategoriaSeleccionada = categoria;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(noticias.Count / (double)pageSize);

            return View(paginatedNoticias);
        }

        private async Task<List<Internacionales>> ObtenerNoticiasInternacionales(string categoria)
        {
            var noticias = new List<Internacionales>();
            var url = categorias[categoria];

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response);

                var articles = htmlDocument.DocumentNode.SelectNodes("//article");

                if (articles != null)
                {
                    foreach (var article in articles)
                    {
                        var titulo = article.SelectSingleNode(".//h2")?.InnerText.Trim();
                        var descripcion = article.SelectSingleNode(".//p")?.InnerText.Trim();
                        var link = article.SelectSingleNode(".//a")?.GetAttributeValue("href", string.Empty);
                        var imagenUrl = article.SelectSingleNode(".//img")?.GetAttributeValue("src", string.Empty);
                        var fecha = article.SelectSingleNode(".//time")?.InnerText.Trim();

                        if (!string.IsNullOrEmpty(titulo) && !string.IsNullOrEmpty(link))
                        {
                            noticias.Add(new Internacionales
                            {
                                Titulo = titulo,
                                Descripcion = descripcion,
                                Url = link,
                                ImagenUrl = imagenUrl,
                                Fecha = fecha
                            });
                        }
                    }
                }
            }

            return noticias;
        }
    }
}
