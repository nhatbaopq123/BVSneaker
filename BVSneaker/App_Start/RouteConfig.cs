using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BVSneaker
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "Shopping Cart",
                url: "shopping-cart",
                defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "Check Out",
                url: "check-out",
                defaults: new { controller = "ShoppingCart", action = "CheckOut", id = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "Topic",
                url: "topic/{alias}-{id}",
                defaults: new { controller = "Product", action = "ProductTopic", id = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "BrandItem",
                url: "brand/{alias}-{id}",
                defaults: new { controller = "Product", action = "ProductBrand", id = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "Product",
                url: "product",
                defaults: new { controller = "Product", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "Brand",
                url: "brand",
                defaults: new { controller = "Product", action = "IndexBrand", alias = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "ProductDetail",
                url: "detail/{alias}-p{id}",
                defaults: new { controller = "Product", action = "Detail", alias = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "Blog",
                url: "blogs",
                defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BVSneaker.Controllers" }
            );
        }
    }
}
