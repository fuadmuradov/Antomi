using Antomi.DataAccsessLayer;
using Antomi.Models;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Antomi.ViewModel;
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
        [HttpPost]
        public IActionResult AddtoBasket(int ColorId, int Quantity)
        {
            ProductColor productColor = context.ProductColors.FirstOrDefault(x => x.Id == ColorId && x.Count>=Quantity);
            if (productColor == null) return NotFound();

            string basket = HttpContext.Request.Cookies["Basket"];
           
            List<BasketCookieItemVM> basketCookieItems;
            if (basket == null)
            {
                basketCookieItems = new List<BasketCookieItemVM>();
                BasketCookieItemVM basketCookieItem = new BasketCookieItemVM()
                {
                    Id = ColorId,
                    Count = Quantity
                };
                basketCookieItems.Add(basketCookieItem);
                
            }
            else
            {
                basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                
                bool isExist = false;
                foreach (var item in basketCookieItems)
                {
                    if (item.Id == ColorId)
                    {
                        if ((item.Count + Quantity) > productColor.Count) return NotFound();
                        else
                        {
                           item.Count = item.Count + Quantity;
                            isExist = true;
                        }
                    }
                }
                if (isExist == false)
                {
                    BasketCookieItemVM basketCookieItem = new BasketCookieItemVM()
                    {
                        Id = ColorId,
                        Count = Quantity
                    };
                    basketCookieItems.Add(basketCookieItem);
                }
            }
            string basketstr = JsonConvert.SerializeObject(basketCookieItems, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            HttpContext.Response.Cookies.Append("Basket", basketstr);

            return PartialView("_CartPartialView");
        }

        [HttpPost]
        public IActionResult DeleteBasketItem(int itemID)
        {
            string basket = HttpContext.Request.Cookies["Basket"];
            List<BasketCookieItemVM> basketCookieItems;
            if (!string.IsNullOrEmpty(basket))
            {
                basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                foreach (var item in basketCookieItems)
                {
                    if (item.Id == itemID)
                    {
                        basketCookieItems.Remove(item);
                        break;
                    }
                }
                string basketstr = JsonConvert.SerializeObject(basketCookieItems, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                HttpContext.Response.Cookies.Append("Basket", basketstr);
            }


            return PartialView("_WishlistPartialView");
        }


        public IActionResult GetCartPartial()
        {
            return PartialView("_CartPartialView");
        }

        public IActionResult ShowBasket()
        {
            string basketstr = HttpContext.Request.Cookies["Basket"];
            List<BasketCookieItemVM> basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basketstr);
            return Json(basket);

        }

    }
}
