using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace SimpleBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            var namespaces = new[] {typeof (SimpleBlog.Controllers.PostsController).Namespace};
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //to use real urls
            //bolg.com/post/123-this-is-a-post
            routes.MapRoute("PostForRealThisTime", "post/{idAndSlug}", new { controller = "Posts", action = "Show" }, namespaces);
            routes.MapRoute("TagForRealThisTime", "tag/{idAndSlug}", new { controller = "Posts", action = "Tag" }, namespaces);
            
            
            //To generate Urls
            routes.MapRoute("Tag", "tag/{id}-{slug}", new { controller = "Posts", action = "Tag"}, namespaces );
            //To generate Urls
            routes.MapRoute("Post", "post/{id}-{slug}", new {controller = "Posts", action = "Show"}, namespaces);
            
            
            
            
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            
            routes.MapRoute("home", "", new {Controller = "Posts", Action = "Index"}, namespaces);
            routes.MapRoute("login", "login", new { Controller = "Login", Action = "Index" });
            routes.MapRoute("logout", "logout", new { Controller = "Login", Action = "logout" });
            //routes.MapRoute("admin", "admin", new { Controller = "Login", Action = "Index" }, namespaces);
            //routes.MapRoute("student", "student", new { Controller = "Login", Action = "Index" }, namespaces);


            routes.MapRoute("Sidebar", "", new {Controller = "Layout", action = "Sidebar"}, namespaces);
        }
    }
}