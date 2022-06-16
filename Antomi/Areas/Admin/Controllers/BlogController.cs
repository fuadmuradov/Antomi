using Antomi.DataAccsessLayer;
using Antomi.Extension;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class BlogController : Controller
    {
        private readonly AntomiDbContext context;
        private readonly IWebHostEnvironment webHost;
        private readonly UserManager<AppUser> userManager;

        public BlogController(AntomiDbContext context, IWebHostEnvironment webHost, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.webHost = webHost;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            List<Blog> blogs = context.Blogs.Include(x => x.AppUser).Include(x => x.Category).Where(x => x.IsDeleted == false).ToList();
            return View(blogs);
        }

        public IActionResult AddBlog()
        {
            ViewBag.Category = context.Categories.ToList();
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBlog(Blog blog)
        {
            ViewBag.Category = context.Categories.ToList();
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
            AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            blog.AppUserId = user.Id;
            if (!ModelState.IsValid) return View(blog);
            if (!blog.Photo.IsImage())
            {
                ModelState.AddModelError("ColorImages", "image type is not Correct");
                return View();
            }
            string folder = @"assets\img\blog\";
            
            blog.Image = blog.Photo.SavaAsync(webHost.WebRootPath, folder).Result;
         
            await context.Blogs.AddAsync(blog);
            await context.SaveChangesAsync();

            return View();
        }

        public IActionResult DeleteBlog(int id)
        {
            Blog blog = context.Blogs.FirstOrDefault(x => x.Id == id);
            blog.IsDeleted = true;
            context.SaveChangesAsync();

            return LocalRedirect("~/Admin/Blog/Index/");
        }

        public IActionResult UpdateBlog()
        {
            ViewBag.Category = context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBlog(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            Blog existBlog = context.Blogs.FirstOrDefault(x => x.Id == blog.Id);

            if (blog.Photo != null)
            {
                try
                {
                    string folder = @"assets\img\blog\";
                    string newImg = await blog.Photo.SavaAsync(webHost.WebRootPath, folder);
                    FileExtension.Delete(webHost.WebRootPath, folder, existBlog.Image);
                    existBlog.Image = newImg;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "unexpected error for Update");
                    return View();
                }
            }

            existBlog.Title = blog.Title;
            existBlog.Description = blog.Description;
            existBlog.CategoryId = blog.CategoryId;
            existBlog.Emphasis = blog.Emphasis;
            await context.SaveChangesAsync();
            return LocalRedirect("~/Admin/Blog/Index/");
        }

    }
}
