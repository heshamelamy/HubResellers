using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{returnUrl}",
                defaults: new { controller = "Account", action = "Login", returnUrl = "/Reseller/Index" },
                namespaces: new[] { "WebApp.Controllers" }
            );
        }
    }
}
