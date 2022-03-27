using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyXe
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Trang Chủ",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Moto", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
