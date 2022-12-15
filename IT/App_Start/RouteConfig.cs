using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "Home",
                 url: "he-thong/{action}/{id}",
                 defaults: new { controller = "Home", action = "trang-chu", id = UrlParameter.Optional }
             );
            routes.MapRoute(
              name: "File",
              url: "file/{action}/{id}",
              defaults: new { controller = "FileManagerData", action = "trang-chu", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "Default",
           url: "{action}/{id}",
           defaults: new { controller = "Login", action = "dang-nhap", id = UrlParameter.Optional }
       );

        }
    }
}
