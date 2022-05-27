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
using Microsoft.AspNetCore.Http;

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

        public async Task<IActionResult> Shop()
        {
            List<Product> products = context.Products.Include(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).Include(x => x.ProductColors).ThenInclude(x => x.Discounts).ToList();
            List<Category> categories = context.Categories.Include(x => x.SubCategories).ToList();
            ShopVM shopVM = new ShopVM()
            {
                Products = products,
                Categories = categories
            };



            string compare = HttpContext.Request.Cookies["Compare"];
            CompareVM compareVM = new CompareVM
            {
                CompareItems = new List<CompareItemVM>()
            };
            List<CompareCookieItemVM> compareCookieItems;
            if (!string.IsNullOrEmpty(compare))
            {
                compareCookieItems = JsonConvert.DeserializeObject<List<CompareCookieItemVM>>(compare);
                foreach (var item in compareCookieItems)
                {
                    ProductColor productColor = await context.ProductColors.Include(x => x.Product).ThenInclude(x => x.SubCategory).Include(x => x.Product).ThenInclude(x => x.PhoneSpecifications).Include(x => x.ProductColorImages).Include(x => x.Discounts).FirstOrDefaultAsync(x => x.Id == item.Id);
                    if (productColor == null) return View(shopVM);
                    Discount discount = await context.Discounts.FirstOrDefaultAsync(x => x.ProductColorId == item.Id && x.IsActive == true);
                    double price = 0;
                    if (discount == null)
                    {
                        price = productColor.Price;
                    }
                    else
                    {
                        price = productColor.Price * (100 - discount.Percent) / 100;
                    }


                    if (productColor.Product.SubCategoryId == 1)
                    {
                        CompareItemVM compareItem = new CompareItemVM()
                        {
                            ProductColor = productColor,
                            Price = Math.Round(price, 2)
                        };
                        compareVM.CompareItems.Add(compareItem);
                    }

                }


            }

            shopVM.CompareVM = compareVM;     
   
            return View(shopVM);
        }

        public IActionResult Details(int id=0)
        {
        
            if(id==0)
            {
                id = context.Products.First().Id;
            }
            Product product = context.Products.Include(x=>x.SubCategory).ThenInclude(x=>x.Category).Include(x => x.PhoneSpecifications).Include(x => x.NotebookSpecifications).Include(x => x.Specifications).Include(x => x.ProductColors).ThenInclude(x=>x.Discounts).Include(x => x.ProductColors).ThenInclude(x=>x.ProductColorImages).FirstOrDefault(x => x.Id == id);// ProductColorIMages
            List<SubcategoryToMarka> subcategoryToMarka = context.SubcategoryToMarkas.Include(x=>x.SubCategory).Include(x=>x.Marka).Where(x=>x.MarkaId==product.MarkaId && x.SubCategoryId==product.SubCategoryId).ToList();
            Marka marka = context.Markas.Include(x=>x.Products).ThenInclude(x=>x.ProductColors).ThenInclude(x=>x.ProductColorImages).Include(x=>x.SubcategoryToMarkas).Include(x => x.Products).ThenInclude(x => x.ProductColors).ThenInclude(x=>x.Discounts).FirstOrDefault(x => x.Id == product.MarkaId);
            List<Product> products = marka.Products.Where(x => x.SubCategoryId == product.SubCategoryId).ToList();
            ViewBag.RelatedProduct = products;
            return View(product);
        }

        public IActionResult AddComment(string comText, string comName, string comEmail, int comProductId)
        {

            return RedirectToAction("Details", "Home", new { id = 1 }) ;
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


        #region ModalQuickView

        [HttpPost]
        public IActionResult QuickView(int id)
        {
            string productId = id.ToString();
            HttpContext.Response.Cookies.Append("Quickview", productId);

            return PartialView("_ModalPartialView");
        }

        public IActionResult GetModalPartial()
        {
            return PartialView("_ModalPartialView");
        }

        #endregion


        #region ShopCategory
        [HttpPost]
        public IActionResult ShopCategory(int id)
        {
            string categoryId = id.ToString();
            HttpContext.Response.Cookies.Append("Category", categoryId);
            return PartialView("_ShopProductPartialView");
        }

        public IActionResult GetShopCategory()
        {
            return PartialView("_ShopProductPartialView");
        }

        [HttpPost]
        public IActionResult ShopProdductSort(int value)
        {
            string sortvalue = value.ToString();
            HttpContext.Session.SetString("Sorting", sortvalue);
            return PartialView("_ShopProductPartialView");
        }
       
        [HttpPost]
        public IActionResult Filter(string text)
        {
            text = text.Replace("$", String.Empty);
            text= text.Replace(" ", String.Empty);
            string[] prices = text.Split("-");
            FilterVM filterVM = new FilterVM()
            {
                Minimum = Convert.ToInt32(prices[0]),
                Maximum = Convert.ToInt32(prices[1])
            };

            string filterstr = JsonConvert.SerializeObject(filterVM, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            HttpContext.Session.SetString("Filter", filterstr);

            return PartialView("_ShopProductPartialView");
        }


        #endregion
    }
}
