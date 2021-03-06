﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Infrastructure;
using SimpleBlog.ViewModel;
using SimpleBlog.Models;

namespace SimpleBlog.Controllers
{
    public class PostsController : Controller
    {
        //
        // GET: /Posts/
        private const int PostsPerPage=5;
        public ActionResult Index(int page=1)
        {
            var baseQuery = Database.Session.Query<Post>().Where(p => p.DeletedAt == null)
                .OrderByDescending(p=>p.CreatedAt);
            var totalPostCount = baseQuery.Count();
            var postIds = baseQuery
                .Skip((page - 1)*PostsPerPage)
                .Take(PostsPerPage)
                .Select(t => t.Id).ToArray();

            var posts = baseQuery
                .Where(p => postIds.Contains(p.Id))
                .FetchMany(t=>t.Tags)
                .Fetch(u=>u.User).ToList();


            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(posts, totalPostCount, page,PostsPerPage)  
            });
        }

        public ActionResult Show(string idAndSlug)
        {
            var parts = SeprateIdAndSlug(idAndSlug);
            if (parts == null)
                return HttpNotFound();
            
            var post = Database.Session.Load<Post>(parts.Item1);
            
            if (post==null || post.IsDeleted)
                return HttpNotFound();

            if (!post.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                //to move permanent with new status code... redirect route permanent
                return RedirectToRoutePermanent("Post", new {id = parts.Item1, slug = post.Slug});

            return View(new PostsShow
            {
                Post = post
            });

        }



        public ActionResult Tag(string idAndSlug, int page=1)
        {
            var parts = SeprateIdAndSlug(idAndSlug);
            if (parts == null)
                return HttpNotFound();

            var tag = Database.Session.Load<Tag>(parts.Item1);

            if (tag == null )
                return HttpNotFound();

            if (!tag.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                //to move permanent with new status code... redirect route permanent
                return RedirectToRoutePermanent("Tag", new { id = parts.Item1, slug = tag.Slug });

            var totalPostCount = tag.Posts.Count();
            var PostIds = tag.Posts.Skip((page - 1)*PostsPerPage)
                .Take(PostsPerPage)
                .Where(t => t.DeletedAt == null)
                .Select(t => t.Id).ToArray();


            var posts =Database.Session.Query<Post>()
                .OrderByDescending(b=>b.CreatedAt)
                .Where(t=>PostIds.Contains(t.Id))
                .FetchMany(f=>f.Tags)
                .Fetch(f=>f.User)
                .ToList();
                
            
            return View(new PostsTag
            {
                Tag=tag,
                Posts=new PagedData<Post>(posts,totalPostCount,page,PostsPerPage)
            });

        }



        private System.Tuple<int, string> SeprateIdAndSlug(string idAndSlug)
        {
            var matches = Regex.Match(idAndSlug, @"^(\d+)\-(.*)?$");
            if (!matches.Success)
                return null;
            var id = int.Parse(matches.Result("$1"));
            var slug = matches.Result("$2");
            return Tuple.Create(id, slug);
        }
    }
}
