using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NHibernate.Linq;
using SimpleBlog.Models;
using SimpleBlog.ViewModel;

namespace SimpleBlog.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SimpleBlog.ViewModel.Auth form, string returnUrl)
        {


            var user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == form.UserName);

            if (user==null)
             SimpleBlog.Models.User.FakeHash();
            
            
            if ((user == null) || (!user.CheckPassword(form.Password)))
                ModelState.AddModelError("Username", "username and password is incorrect");

        

            if (!ModelState.IsValid)
            {

                return View(form);
            }

            
            FormsAuthentication.SetAuthCookie(user.Username, true);


            //return View();
            //return Content("Valid Login: Hi There, " + form.UserName);

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToRoute("home");

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("home");
        }

    }
}
