using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RadioConexionLatam.Models
{
    public class Internacionales
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public string ImagenUrl { get; set; }
        public string Fecha { get; set; }

        public static async Task<List<Internacionales>> ObtenerUltimasCancionesAsync()
        {
            var url = "https://myanimelist.net/anime/season"; // URL de la página de temporada de MyAnimeList
            var canciones = new List<Internacionales>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
                var response = await httpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(response);

                // Ajusta el selector de acuerdo con la estructura HTML del sitio web
                var cancionesHtml = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'seasonal-anime')]//a[@class='link-title']");

                if (cancionesHtml != null)
                {
                    int count = 0;
                    foreach (var node in cancionesHtml)
                    {
                        if (count >= 10) break;

                        var titulo = node.InnerText.Trim();
                        var link = node.GetAttributeValue("href", string.Empty);
                        canciones.Add(new Internacionales { Titulo = titulo, Url = link });

                        count++;
                    }
                }
            }

            return canciones;
        }
    }
}