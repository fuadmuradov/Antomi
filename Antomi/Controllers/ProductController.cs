using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Antomi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Controllers
{
    public class ProductController : Controller
    {
        private readonly AntomiDbContext context;

        public ProductController(AntomiDbContext context)
        {
            this.context = context;
        }

        public IActionResult Wishlist()
        {
           string wishlist = HttpContext.Request.Cookies["Wishlist"];
            List<WishlistCookieItemVM> wishlistitems;
            WishlistVM wishlistVM = new WishlistVM()
            {
                wishlistItems = new List<WishlistItemVM>()
            };
            if (!string.IsNullOrEmpty(wishlist))
            {
                wishlistitems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlist);
                foreach (var item in wishlistitems)
                {
                    double price = 0;
                    ProductColor productColor = context.ProductColors.FirstOrDefault(x => x.Id == item.Id && x.Count > 0); 
                    if(productColor != null)
                    {
                        Discount discount = productColor.Discounts.FirstOrDefault(x => x.IsActive == true);
                        if (discount != null)
                        {
                            price = productColor.Price * (100 - discount.Percent) / 100;
                        }
                        else
                        {
                            price = productColor.Price;
                        }
                        WishlistItemVM wishlistItemVM = new WishlistItemVM()
                        {
                            ProductColor = productColor,
                            Price = Math.Round(price, 2)
                        };
                        wishlistVM.wishlistItems.Add(wishlistItemVM);
                    }
                }
            }

            return View(wishlistVM);
        }

        public IActionResult AddWishlist()
        {

            return PartialView("");
        }
    }
}
