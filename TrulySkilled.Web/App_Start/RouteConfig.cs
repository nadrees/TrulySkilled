﻿using System.Web.Mvc;
using System.Web.Routing;

namespace TrulySkilled.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{controller}/RegisterGame"); // disallow registering games from outside
            routes.IgnoreRoute("{controller}/RecordResults"); // disallow recording game results from outside

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}