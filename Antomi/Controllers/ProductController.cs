using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Antomi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //**********************Wishlist*************************
        #region Wishlist
        public IActionResult Wishlist()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddWishlist(int ColorID)
        {
            string wishlist = HttpContext.Request.Cookies["Wishlist"];
            ProductColor productColor = context.ProductColors.FirstOrDefault(x => x.Id == ColorID);
            if (productColor == null) return PartialView("_WishlistPartialView");
            List<WishlistCookieItemVM> wishlistCookieItems;
            if (string.IsNullOrEmpty(wishlist))
            {
                wishlistCookieItems = new List<WishlistCookieItemVM>();
                WishlistCookieItemVM wishlistCookieItem = new WishlistCookieItemVM()
                {
                    Id = ColorID
                };
                wishlistCookieItems.Add(wishlistCookieItem);
            }
            else
            {
                wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlist);
                bool isExist = false;
                foreach (var item in wishlistCookieItems)
                {
                    if (item.Id == ColorID)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    WishlistCookieItemVM wishlistCookieItem = new WishlistCookieItemVM()
                    {
                        Id = ColorID
                    };
                    wishlistCookieItems.Add(wishlistCookieItem);
                }

            }

            string wishliststr = JsonConvert.SerializeObject(wishlistCookieItems, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            HttpContext.Response.Cookies.Append("Wishlist", wishliststr);

            return PartialView("_WishlistPartialView");
        }

        [HttpPost]
        public IActionResult DeleteWishlistItem(int itemID)
        {
            string wishlist = HttpContext.Request.Cookies["Wishlist"];
            List<WishlistCookieItemVM> wishlistCookieItems;
            if (!string.IsNullOrEmpty(wishlist))
            {
                wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlist);

                foreach (var item in wishlistCookieItems)
                {
                    if (item.Id == itemID)
                    {
                        wishlistCookieItems.Remove(item);
                        break;
                    }
                }

                string wishliststr = JsonConvert.SerializeObject(wishlistCookieItems, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                HttpContext.Response.Cookies.Append("Wishlist", wishliststr);
            }

            return PartialView("_WishlistPartialView");
        }

        public IActionResult GetWishlistPartial()
        {
            return PartialView("_WishlistPartialView");
        }

        public IActionResult GetWishlistTablePartial()
        {
            return PartialView("_WishlistTablePartialView");
        }
        #endregion


        #region Compare
        public IActionResult Compare()
        {
            return View();
        }

        public async Task<IActionResult> AddCompare(int ColorID)
        {
            ProductColor productColor = await context.ProductColors.FirstOrDefaultAsync(x => x.Id == ColorID);
            if (productColor == null) return PartialView("_ComparePartialView");

            string compare = HttpContext.Request.Cookies["Compare"];
            List<CompareCookieItemVM> compareCookieItems;
            if (string.IsNullOrEmpty(compare))
            {
                compareCookieItems = new List<CompareCookieItemVM>();
                CompareCookieItemVM compareCookieItem = new CompareCookieItemVM()
                {
                    Id = ColorID
                };
                compareCookieItems.Add(compareCookieItem);
            }
            else
            {
                compareCookieItems = JsonConvert.DeserializeObject<List<CompareCookieItemVM>>(compare);
                bool isExist = false;
                foreach (var item in compareCookieItems)
                {
                    if (item.Id == ColorID)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (isExist == false)
                {

                    CompareCookieItemVM compareCookieItem = new CompareCookieItemVM()
                    {
                        Id = ColorID
                    };
                    compareCookieItems.Add(compareCookieItem);
                }
            }
            string comparestr = JsonConvert.SerializeObject(compareCookieItems);
            HttpContext.Response.Cookies.Append("Compare", comparestr);

            return PartialView("_ComparePartialView");
        }


        public IActionResult DeleteCompareItem(int itemID)
        {
            string compare = HttpContext.Request.Cookies["Compare"];
            List<CompareCookieItemVM> compareCookieItems;
            if (!string.IsNullOrEmpty(compare))
            {
                compareCookieItems = JsonConvert.DeserializeObject<List<CompareCookieItemVM>>(compare);
                foreach (var item in compareCookieItems)
                {
                    if (item.Id == itemID)
                    {
                        compareCookieItems.Remove(item);
                        break;
                    }
                }
                string comparestr = JsonConvert.SerializeObject(compareCookieItems);
                HttpContext.Response.Cookies.Append("Compare", comparestr);
            }


            return PartialView("_ComparePartialView");
        }

        public IActionResult GetComparePartial()
        {
            return PartialView("_ComparePartialView");
        }
        #endregion

    }
}
