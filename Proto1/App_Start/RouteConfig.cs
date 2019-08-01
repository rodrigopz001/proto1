using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Proto1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Saludo",
                url: "{controller}/{action}/",
                defaults: new { controller = "Home", action = "Saludo"}
            );

            routes.MapRoute(
                name: "Despedida",
                url: "{controller}/{action}/",
                defaults: new { controller = "Home", action = "Despedida"}
            );

            routes.MapRoute(
                name: "Mensaje",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Mensaje", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CrudIndex",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Crud", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Create",
                url: "{controller}/{action}/",
                defaults: new { controller = "Crud", action = "Create" }
            );

            routes.MapRoute(
                name: "Update",
                url: "{controller}/{action}/",
                defaults: new { controller = "Crud", action = "Update" }
            );

            routes.MapRoute(
                name: "Delete",
                url: "{controller}/{action}/",
                defaults: new { controller = "Crud", action = "Delete" }
            );

        }
    }
}
