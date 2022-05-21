using Antomi.DataAccsessLayer;
using Antomi.Models;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AntomiDbContext context;

        public HomeController(AntomiDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            Product product = context.Products.Include(x=>x.SubCategory).ThenInclude(x=>x.Category).Include(x => x.PhoneSpecifications).Include(x => x.NotebookSpecifications).Include(x => x.Specifications).Include(x => x.ProductColors).ThenInclude(x=>x.Discounts).Include(x => x.ProductColors).ThenInclude(x=>x.ProductColorImages).FirstOrDefault(x => x.Id == id);// ProductColorIMages
            return View(product);
        }

        public IActionResult ProductChangeColor(int ColorId)
        {
           ProductColor productColor = context.ProductColors.Include(x => x.ProductColorImages).Include(x=>x.Discounts).FirstOrDefault(x => x.Id == ColorId);

            return Json(JsonConvert.SerializeObject(productColor, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }
    }
}
