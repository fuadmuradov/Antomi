using Antomi.DataAccsessLayer;
using Antomi.Extension;
using Antomi.Models.Entity;
using Antomi.ViewModel;
using Microsoft.AspNetCore.Hosting;
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
    public class ProductController : Controller
    {
        private readonly AntomiDbContext context;
        private readonly IWebHostEnvironment webHost;

        public ProductController(AntomiDbContext context, IWebHostEnvironment webHost)
        {
            this.context = context;
            this.webHost = webHost;
        }

        public IActionResult Product()
        {
            List<Product> products = context.Products.Include(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).ToList();
            return View(products);
        }

        public IActionResult CreateProduct()
        {
            ViewBag.Marka = context.Markas.ToList();
            ViewBag.SubCategory = context.SubCategories.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            ViewBag.Marka = context.Markas.ToList();
            ViewBag.SubCategory = context.SubCategories.ToList();
            if (!ModelState.IsValid) return View();
            context.Products.Add(product);
            await context.SaveChangesAsync();
            if (product.SubCategoryId == 1) return LocalRedirect("/admin/Product/AddPhoneSpec?id=" + product.Id.ToString());
            else if (product.SubCategoryId == 17) return LocalRedirect("/admin/Product/AddNotebookSpec?id=" + product.Id.ToString());
            else return LocalRedirect("/admin/Product/Specification?id=" + product.Id.ToString());
            // return LocalRedirect("/admin/Product/AddSkill?Tid=" + TempData["teacherId"].ToString());
            return View();
        }

        public IActionResult AddPhoneSpec(int id)
        {
            PhoneSpecification phoneSpec = new PhoneSpecification()
            {
                ProductId = id
            };
            return View(phoneSpec);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPhoneSpec(PhoneSpecification phoneSpecification)
        {
            if (!ModelState.IsValid) return View();
            phoneSpecification.Id = 0;
            context.PhoneSpecifications.Add(phoneSpecification);
             context.SaveChanges();
            return LocalRedirect("/admin/Product/Specification?id=" + phoneSpecification.ProductId.ToString());
        }


        public IActionResult AddNotebookSpec(int id)
        {
            NotebookSpecification notebookspecification = new NotebookSpecification
            {
                ProductId = id
            };
            return View(notebookspecification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNotebookSpec(NotebookSpecification notebook)
        {
            if (!ModelState.IsValid) return View();
            context.NotebookSpecifications.Add(notebook);
            await context.SaveChangesAsync();
            return LocalRedirect("/admin/Product/Specification?id=" + notebook.ProductId.ToString());
        }

        public IActionResult Specification(int id)
        {
            Specification specific = new Specification()
            {
                ProductId = id
            };
            return View(specific);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Specification(Specification specification)
        {
            if (!ModelState.IsValid) return View();
            specification.Id = 0;
            context.Specifications.Add(specification);
            await context.SaveChangesAsync();

            return LocalRedirect("/admin/Product/Specification?id=" + specification.ProductId.ToString());
        }

        public IActionResult AddProductColor(int id)
        {
            ColorImageVM pcolor = new ColorImageVM()
            {
                ProductId = id
            };
            return View(pcolor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductColor(Antomi.ViewModel.ColorImageVM colorImage)
        {
            if (!ModelState.IsValid) return View();
            ProductColor productColor = new ProductColor()
            {
                Name = colorImage.ColorName,
                Price = colorImage.Price,
                Count = colorImage.Quantity,
                ProductId = colorImage.ProductId
            };


            context.ProductColors.Add(productColor);
            await context.SaveChangesAsync();
            int counter = 0;
            foreach (var file in colorImage.ColorImages)
            {
                counter++;
                ProductColorImage image = new ProductColorImage();
                if (!file.IsImage())
                {
                    ModelState.AddModelError("ColorImages", "image type is not Correct");
                    return View();
                }

                string folder = @"image\product\";
                image.Image = file.SavaAsync(webHost.WebRootPath, folder).Result;
                if (counter == 1) image.IsMain = true;
                else
                    image.IsMain = false;

                image.ProductColorId = productColor.Id;
                context.ProductColorImages.Add(image);
                await context.SaveChangesAsync();

                if (counter == 5) break;
            }

            return LocalRedirect("/admin/Product/AddProductColor?id=" + colorImage.ProductId.ToString());
        }



    }
}
