using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
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
    public class DiscountController : Controller
    {
        private readonly AntomiDbContext context;

        public DiscountController(AntomiDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<Discount> discounts = context.Discounts.Include(x=>x.ProductColor).ThenInclude(x=>x.Product).ToList();
            return View(discounts);
        }

        [HttpGet("{id}")]
        public IActionResult AddDiscount(int id)
        {
            ViewBag.ProductColors = context.ProductColors.Where(x => x.ProductId == id).ToList();
            TempData["productID"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiscauntAdd(Discount discount)
        {
            int productid = Convert.ToInt32(TempData["productID"]);
            //   ViewBag.ProductColors = context.ProductColors.Where(x => x.ProductId == id).ToList();
            if (!ModelState.IsValid) return View("AddDiscount", discount);
            discount.IsActive = true;
            await context.Discounts.AddAsync(discount);
            await context.SaveChangesAsync();

            return LocalRedirect("~/Admin/Discount/Index");
        }

        public IActionResult DeleteDiscount(int id)
        {
            Discount discount = context.Discounts.FirstOrDefault(x => x.Id == id);
            if (discount == null) return NotFound();
            context.Discounts.Remove(discount);
            context.SaveChanges();
            return LocalRedirect("/Admin/Discount/Index");
        }

        public IActionResult UpdateDiscount(int id)
        {
            Discount discount = context.Discounts.Include(x=>x.ProductColor).ThenInclude(x=>x.Product).FirstOrDefault(x => x.Id == id);
            ViewBag.ProductColors = context.ProductColors.Where(x => x.ProductId == discount.ProductColor.Product.Id).ToList();
            if (discount == null) return NotFound();

            return View(discount);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDiscount(Discount discount)
        {
            if (!ModelState.IsValid) return View(discount);
            Discount dbDiscount = context.Discounts.FirstOrDefaultAsync(x => x.Id == discount.Id).Result;
            dbDiscount.StartDate = discount.StartDate;
            dbDiscount.EndDate = discount.EndDate;
            dbDiscount.ProductColorId = discount.ProductColorId;
            dbDiscount.Percent = discount.Percent;
            await context.SaveChangesAsync();

            return LocalRedirect("/Admin/Discount/Index");
        }

        public IActionResult DealOfMonth(int id)
        {
            Discount discount = context.Discounts.FirstOrDefault(x => x.Id == id);
            if (discount.DealofMonth == true)
            {
                discount.DealofMonth = false;
            }
            else
            {
                discount.DealofMonth = true;
            }

            context.SaveChanges();
            return LocalRedirect("/Admin/Discount/Index");
        }

    }
}
