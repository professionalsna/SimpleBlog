﻿using System;
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
    [Authorize(Roles="admin")]
    [SelectedTab("users")]
    public class UserController : Controller
    {
        //
        // GET: /admin/User/

        public ActionResult Index()
        {
            return View(new UserIndex
            {
                Users=Database.Session.Query<User>().ToList()
            } );
        }

        public ActionResult New()
        {
            return View(new UserNew
            {
                Roles=Database.Session.Query<Role>().Select(role=> new RoleCheckBox
                {
                    Id=role.Id,
                    Name=role.Name,
                    IsChecked=false

                }).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(UserNew form)
        {

            var user = new User();
            SyncRoles(form.Roles, user.Roles);



            if (Database.Session.Query<User>().Any(u => u.Username== form.Username))
            {
                ModelState.AddModelError("Username", "Username must be unique");
            }

            if (!ModelState.IsValid)
                return View(form);


            //var user = new User
            //{
            //    Email=form.Email,
            //    Username=form.Username,
            //    //PasswordHash=

            //};
            user.Email = form.Email;
            user.Username = form.Username;
            user.SetPassword(form.Password);

            Database.Session.Save(user);
            return RedirectToAction("index");

        }


        public ActionResult Edit(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user== null)
            return HttpNotFound();

            return View(new UserEdit
            {
                Username = user.Username,
                Email = user.Email,
                
                Roles=Database.Session.Query<Role>().Select(role=> new RoleCheckBox
                {
                    Id=role.Id,
                    Name=role.Name,
                    IsChecked=user.Roles.Contains(role)

                }).ToList()
            });

        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserEdit form)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            SyncRoles(form.Roles, user.Roles);

            //if (Database.Session.Query<User>().Any(u => u.Username == form.Username && u.Id == id))
            //    ModelState.AddModelError("username", "Username must be unique");

            if (!ModelState.IsValid)
                return View(form);


            user.Username = form.Username;
            user.Email = form.Email;
            Database.Session.Update(user);

            return RedirectToAction("index");

       }


        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            return View(new UserResetPassword
            {
                Username = user.Username
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id, UserResetPassword form)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            form.Username = user.Username;

            if (!ModelState.IsValid)
                return View(form);


            user.SetPassword(form.Password);
            
            Database.Session.Update(user);
            return RedirectToAction("index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
                return HttpNotFound();

            Database.Session.Delete(user);
            return RedirectToAction("index");
        }

        private void SyncRoles(IList<RoleCheckBox> checkBoxes, IList<Role> roles)
        {
            var selectedRoles = new List<Role>();

            foreach (var  role in Database.Session.Query<Role>())
            {
                var checkbox = checkBoxes.Single(c => c.Id == role.Id);
                checkbox.Name = role.Name;

                if (checkbox.IsChecked)
                selectedRoles.Add(role);
                
            }

            foreach (var toAdd in selectedRoles.Where(t=>!roles.Contains(t)))
            roles.Add(toAdd);
            
            foreach (var toRemove in roles.Where(t=>! selectedRoles.Contains(t)).ToList())
            roles.Remove(toRemove);
            
        }
    }
}
