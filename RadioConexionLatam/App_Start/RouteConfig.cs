using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RadioConexionLatam
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "EditarAnuncio",
                url: "Anuncio/EditarAnuncio/{id}",
                defaults: new { controller = "Anuncio", action = "EditarAnuncio", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "DetallesAnuncio",
                url: "Anuncio/DetallesAnuncio",
                defaults: new { controller = "Anuncio", action = "DetallesAnuncio" }
            );
        }
    }
}
