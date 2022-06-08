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
            List<Blog> blogs = context.Blogs.ToList();
            ViewBag.Category = context.Categories.ToList();
            ViewBag.LatestBlogs = context.Blogs.OrderBy(x => x.CreatedAt).Take(5).ToList();
            ViewBag.BlogComments = context.BlogComments.Include(x => x.AppUser).OrderBy(x => x.CreatedAt).Take(4).ToList();
            return View(blogs);
        }

        public IActionResult Details(int id=0)
        {
            if(id==0)
            {
                id = context.Blogs.First().Id;
            }
            Blog blog = context.Blogs.Include(x=>x.AppUser).FirstOrDefault(x => x.Id == id);
            if (blog == null) return NotFound();
            ViewBag.BlogComments = context.BlogComments.Include(x => x.ReplyComments).ThenInclude(x=>x.AppUser).Include(x=>x.AppUser).Where(x => x.BlogId == id).ToList();
            ViewBag.Blogs = context.Blogs.Where(x => x.CategoryId == blog.CategoryId).Take(3).ToList();
            ViewBag.LatestBlogs = context.Blogs.OrderBy(x => x.CreatedAt).Take(5).ToList();
            ViewBag.Category = context.Categories.ToList();
            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> AddBlogComment(string comment, int blogId)
        {
            if(comment==null && blogId==0) return RedirectToAction(nameof(Index), "Blog", new { id = blogId });
            BlogComment blogComment;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
                blogComment = new BlogComment()
                {
                    AppUserId = user.Id,
                    Text = comment,
                    BlogId = blogId
                };
                context.BlogComments.Add(blogComment);
                await context.SaveChangesAsync();
            }
        
            return RedirectToAction(nameof(Details), "Blog", new {id=blogId });
        }



        [HttpPost]
        public async Task<IActionResult> AddReplyComment(string comment, int blogId)
        {
            if (comment == null && blogId == 0) return RedirectToAction(nameof(Details), "Blog", new { id = blogId });
            ReplyComment replyComment;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
                replyComment = new ReplyComment()
                {
                    AppUserId = user.Id,
                    Text = comment,
                    BlogCommentId = blogId
                };
                context.ReplyComments.Add(replyComment);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), "Blog", new { id = blogId });
        }


    }
}
