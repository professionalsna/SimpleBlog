using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Models;
using SimpleBlog.ViewModel;

namespace SimpleBlog.Controllers
{
    public class LayoutController : Controller
    {
        //
        // GET: /Layout/
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return View(new LayoutSidebar
            {
                IsLogedIn = Auth.User != null,
                UserName = Auth.User != null ? Auth.User.Username : "",
                IsAdmin = User.IsInRole("admin"),
                Tags = Database.Session.Query<Tag>().Select(tag => new
                {
                    tag.Id,
                    tag.Name,
                    tag.Slug,
                    PostCount = tag.Posts.Count
                }).Where(t => t.PostCount > 0).OrderByDescending(p => p.PostCount).Select(
                    tag => new SidebarTag(tag.Id, tag.Name, tag.Slug, tag.PostCount)
                    ).ToList()
            });
        }

    }
}
