using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Areas.admin.ViewModels;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;

namespace SimpleBlog.Areas.admin.Controllers
{
    [SelectedTab("roles")]
    public class RolesController : Controller
    {
        //
        // GET: /admin/Roles/

        public ActionResult Index()
        {
            
            return View(new RoleIndex
            {
                Roles = Database.Session.Query<Role>().ToList()
            });
        }

        public ActionResult New()
        {
            return View(new vmRole{});
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(vmRole form)
        {


            var roles = new Role();
            
            if (Database.Session.Query<Role>().Any(r => r.Name == form.Name))
            {
                ModelState.AddModelError("RoleName", "RoleName must be unique");
            }

            if (!ModelState.IsValid)
                return View(form);


            
            roles.Name = form.Name;
            
            Database.Session.Save(roles);
            return RedirectToAction("index");
            
    }

        public ActionResult Edit(int id)
        {

            var roles = Database.Session.Load<Role>(id);
            if (roles == null)
                return HttpNotFound();

            return View(new vmRole
            {
                Name=roles.Name
                
            });
            
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, vmRole form)
        {

            var role = Database.Session.Load<Role>(id);
            if (role == null)
                return HttpNotFound();

            if (!ModelState.IsValid)
                return View(form);

            role.Name = form.Name;
            
            Database.Session.Update(role);

            return RedirectToAction("index");
        }

        public ActionResult ViewRolesUsers(int id)
        {
            var roles = Database.Session.Load<Role>(id);
            if (roles == null)
                return HttpNotFound();

            return View(new viewRoleUsers
            {
                Name = roles.Name,
                Users= roles.Users
                
            });
        }


    }
}
