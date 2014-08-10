using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mattjgrant
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "List",
                url: "Checklist/List/{checklistID}",
                defaults: new { controller = "Checklist", action = "List", checklistID = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Checklist",
            //    url: "Checklist/{action}/{checklistID}",
            //    defaults: new { controller = "Checklist", action = "{action}", checklistID = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{checklistID}",
                defaults: new { controller = "Checklist", action = "Index", checklistID = UrlParameter.Optional }
            );
        }
    }
}
