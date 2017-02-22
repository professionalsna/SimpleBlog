using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;
using SimpleBlog.Models;

namespace SimpleBlog.Areas.admin.ViewModels
{
    public class RoleIndex
    {
        public IEnumerable<Role> Roles { get; set; }
    }

    public class vmRole
    {
      [Required, MaxLength(128)]
        public string Name { get; set; }

    }

    public class viewRoleUsers
    {
        public string Name { get; set; }
        public IList<User> Users { get; set; }
    }



}