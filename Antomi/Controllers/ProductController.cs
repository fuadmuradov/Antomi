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

        public IActionResult Cart()
        {
            string basket = HttpContext.Request.Cookies["Basket"];
            BasketVM basketVM = new BasketVM()
            {
                TotalPrice = 0,
                Count = 0,
                BasketItems = new List<BasketItemVM>()
            };

            if (!string.IsNullOrEmpty(basket))
            {
                List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                foreach (BasketCookieItemVM item in basketCookieItems)
                {
                    ProductColor productColor = context.ProductColors.Include(x => x.Product).Include(x => x.ProductColorImages).Include(x => x.Discounts).FirstOrDefault(x => x.Id == item.Id && x.Count > 0);
                    if (productColor != null)
                    {
                        double price = 0;
                        double itemMainPrice = 0;
                        Discount discount = productColor.Discounts.FirstOrDefault(x => x.IsActive == true);
                        if (discount != null)
                        {
                            itemMainPrice = productColor.Price * (100 - discount.Percent) / 100;
                            price = itemMainPrice * item.Count;
                        }
                        else
                        {
                            itemMainPrice = productColor.Price;
                            price = itemMainPrice * item.Count;
                        }
                        BasketItemVM basketItem = new BasketItemVM()
                        {
                            ProductColor = productColor,
                            Count = item.Count,
                            Price = Math.Round(itemMainPrice, 2)
                        };

                        basketVM.BasketItems.Add(basketItem);
                        basketVM.Count++;


                        basketVM.TotalPrice += Math.Round(price, 2);
                    }


                }


            }


            return View(basketVM);
        }

        public IActionResult Checkout()
        {
            string basket = HttpContext.Request.Cookies["Basket"];
            BasketVM basketVM = new BasketVM()
            {
                TotalPrice = 0,
                Count = 0,
                BasketItems = new List<BasketItemVM>()
            };
            if (!string.IsNullOrEmpty(basket))
            {
                List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                foreach (BasketCookieItemVM item in basketCookieItems)
                {
                    ProductColor productColor = context.ProductColors.Include(x => x.Product).Include(x => x.ProductColorImages).Include(x => x.Discounts).FirstOrDefault(x => x.Id == item.Id && x.Count > 0);
                    if (productColor != null)
                    {
                        double price = 0;
                        double itemMainPrice = 0;
                        Discount discount = productColor.Discounts.FirstOrDefault(x => x.IsActive == true);
                        if (discount != null)
                        {
                            itemMainPrice = productColor.Price * (100 - discount.Percent) / 100;
                            price = itemMainPrice * item.Count;
                        }
                        else
                        {
                            itemMainPrice = productColor.Price;
                            price = itemMainPrice * item.Count;
                        }
                        BasketItemVM basketItem = new BasketItemVM()
                        {
                            ProductColor = productColor,
                            Count = item.Count,
                            Price = Math.Round(itemMainPrice, 2)
                        };

                        basketVM.BasketItems.Add(basketItem);
                        basketVM.Count++;
                        basketVM.TotalPrice += Math.Round(price, 2);
                    }
                }
            }

            CheckoutVM checkoutVM = new CheckoutVM()
            {
                BasketVM = basketVM,
            };
            return View(checkoutVM);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Address address)
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (!ModelState.IsValid) return RedirectToAction("Checkout", "Product");
                Address addressDb = new Address()
                {
                    Country = address.Country,
                    Street = address.Street,
                    Town = address.Town,
                    Email = address.Email,
                    Name = address.Name,
                    Surname = address.Surname,
                    Phone = address.Phone
                };
                
                string basket = HttpContext.Request.Cookies["Basket"];
                BasketVM basketVM = new BasketVM()
                {
                    TotalPrice = 0,
                    Count = 0,
                    BasketItems = new List<BasketItemVM>()
                };
                if (!string.IsNullOrEmpty(basket))
                {
                    List<BasketCookieItemVM> basketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(basket);
                    foreach (BasketCookieItemVM item in basketCookieItems)
                    {
                        ProductColor productColor = context.ProductColors.Include(x => x.Product).Include(x => x.ProductColorImages).Include(x => x.Discounts).FirstOrDefault(x => x.Id == item.Id && x.Count > 0);
                        if (productColor != null)
                        {
                            if (productColor.Count < item.Count) item.Count = productColor.Count;
                            double price = 0;
                            double itemMainPrice = 0;
                            Discount discount = productColor.Discounts.FirstOrDefault(x => x.IsActive == true);
                            if (discount != null)
                            {
                                itemMainPrice = productColor.Price * (100 - discount.Percent) / 100;
                                price = itemMainPrice * item.Count;
                            }
                            else
                            {
                                itemMainPrice = productColor.Price;
                                price = itemMainPrice * item.Count;
                            }
                            BasketItemVM basketItem = new BasketItemVM()
                            {
                                ProductColor = productColor,
                                Count = item.Count,
                                Price = Math.Round(itemMainPrice, 2)
                            };

                            basketVM.BasketItems.Add(basketItem);
                            basketVM.Count++;
                            basketVM.TotalPrice += Math.Round(price, 2);
                        }
                    }

                    await context.Addresses.AddAsync(address);
                    Order order = new Order()
                    {
                        Address = addressDb,
                        TotalPrice = basketVM.TotalPrice
                    };

                    await context.Orders.AddAsync(order);

                    foreach (var item in basketVM.BasketItems)
                    {
                        ProductColor productColor = await context.ProductColors.FirstOrDefaultAsync(x => x.Id == item.ProductColor.Id);

                        OrderItem orderItem = new OrderItem()
                        {
                            ProductId = item.ProductColor.ProductId,
                            Order = order,
                            Price = item.Price,
                            Quantity = item.Count
                        };
                        productColor.Count = productColor.Count - item.Count;
                        await context.OrderItems.AddAsync(orderItem);
                        await context.SaveChangesAsync();
                    }
                    
                    HttpContext.Response.Cookies.Delete("Basket");
                    return RedirectToAction(nameof(Cart), "Product");
                }
                else
                {
                    return RedirectToAction(nameof(Checkout), "Product");
                }





            }

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

        public IActionResult ShowCompare()
        {
            string basketstr = HttpContext.Request.Cookies["Compare"];
            List<CompareCookieItemVM> basket = JsonConvert.DeserializeObject<List<CompareCookieItemVM>>(basketstr);
            return Json(basket);

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

        public IActionResult GetComparePhonePartial()
        {
            return PartialView("_ComparePartialView");
        }

        public IActionResult GetCompareNotebookPartial()
        {
            return PartialView("_CompareNotebookPartialView");
        }
        #endregion

    }
}
