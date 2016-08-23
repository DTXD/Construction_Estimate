using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Du_Toan_Xay_Dung
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*
            //dinh nghia lai URL cho no gon
            //rewrite url
            routes.MapRoute(
                //dat cai ten gi cung dc cho de nho
                name: "TT_D_KLCongViec_Index",
                //url thay the
                url: "{controller}/{action}",
                //controller va action no tro den
                defaults: new { controller = "TT_D_KLCongViec", action = "Index",id = UrlParameter.Optional}
            );
            */
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            /*
            routes.MapRoute(
                //dat cai ten gi cung dc cho de nho
                name: "TT_D_KLCongViec",
                //url thay the
                url: "{controller}/{action}",
                //controller va action no tro den
                defaults: new { controller = "TT_D_KLCongViec", action = "Index", id = UrlParameter.Optional }
            );
             */
        }
    }
}
