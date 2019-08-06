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
                name: "Login",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
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
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Crud", action = "Update", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Delete",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Crud", action = "Delete", id = UrlParameter.Optional }
            );

        }
    }
}
