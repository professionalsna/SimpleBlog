using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleBlog.ViewModel
{


    public class SidebarTag
    {
        public int Id { get; private set; }
        public string Name{ get; private set; }
        public string Slug { get; private set; }
        public int PostCount { get; private set; }

        public SidebarTag(int id, string name, string slug, int postcount)
        {
            Id = id;
            Name = name;
            Slug = slug;
            PostCount = postcount;
        }

    }

    public class LayoutSidebar
    {
        public bool IsLogedIn { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public IEnumerable<SidebarTag> Tags { get; set; }

    }
}