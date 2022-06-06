using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Controllers
{
    public class BlogController : Controller
    {
        private readonly AntomiDbContext context;
        private readonly UserManager<AppUser> userManager;

        public BlogController(AntomiDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            Blog blog = context.Blogs.Include(x=>x.AppUser).FirstOrDefault(x => x.Id == id);
            if (blog == null) return NotFound();
        //    ViewBag.BlogComments = context.BlogComments.Include(x => x.ReplyComments).Where(x => x.Id == id).ToList();
            ViewBag.Blogs = context.Blogs.Where(x => x.CategoryId == blog.CategoryId).Take(3).ToList();
            ViewBag.LatestBlogs = context.Blogs.OrderBy(x => x.CreatedAt).Take(5).ToList();
            ViewBag.Category = context.Categories.ToList();
            return View(blog);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddBlogComment(string comment, int blogId)
        //{
        //    BlogComment blogComment;
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
        //        blogComment = new BlogComment()
        //        {
        //            AppUserId1 = user.Id,
        //            Text = comment,
        //            BlogId = blogId,
        //            AppUserId = 0
        //        };
        //    }

        //    return View();
        //}


    }
}
