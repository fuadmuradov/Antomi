using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Controllers
{
    public class BlogController : Controller
    {
        private readonly AntomiDbContext context;

        public BlogController(AntomiDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            Blog blog = context.Blogs.FirstOrDefault(x => x.Id == id);
            if (blog == null) return NotFound();

            ViewBag.Blogs = context.Blogs.Where(x => x.CategoryId == blog.CategoryId).Take(3).ToList();
            ViewBag.LatestBlogs = context.Blogs.OrderBy(x => x.CreatedAt).Take(5);
            ViewBag.Category = context.Categories.ToList();
            return View(blog);
        }
    }
}
