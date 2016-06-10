using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace BaseOfTalents.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
        }
    }
}