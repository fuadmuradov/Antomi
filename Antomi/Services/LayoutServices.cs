﻿using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Antomi.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Services
{
    public class LayoutServices
    {
        private readonly AntomiDbContext context;
        private readonly IHttpContextAccessor httpContext;

        public LayoutServices(AntomiDbContext context, IHttpContextAccessor httpContext)
        {
            this.context = context;
            this.httpContext = httpContext;
        }

        public async Task<BasketVM> ShowBasket()
        {
            string basket = httpContext.HttpContext.Request.Cookies["Basket"];
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
                            Price = Math.Round(itemMainPrice,2)                             
                        };

                        basketVM.BasketItems.Add(basketItem);
                        basketVM.Count++;


                        basketVM.TotalPrice += Math.Round(price, 2);
                    }


                }


            }

            return basketVM;
        }

        public async Task<int> WishlistCount()
        {
            int count = 0;
            string wishlist = httpContext.HttpContext.Request.Cookies["Wishlist"];
            List<WishlistCookieItemVM> wishlistCookieItems;
            if (!string.IsNullOrEmpty(wishlist))
            {
                wishlistCookieItems = JsonConvert.DeserializeObject<List<WishlistCookieItemVM>>(wishlist);
                count = wishlistCookieItems.Count;
            }

            return count;
        }

        public async Task<WishlistVM> WishlistTable()
        {
            string wishlist =httpContext.HttpContext.Request.Cookies["Wishlist"];
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
                    ProductColor productColor = context.ProductColors.Include(x => x.ProductColorImages).Include(x => x.Discounts).Include(x => x.Product).FirstOrDefault(x => x.Id == item.Id && x.Count > 0);
                    if (productColor != null)
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

            return wishlistVM;
        }

        public async Task<CompareVM> ComparePhoneList()
        {
            string compare = httpContext.HttpContext.Request.Cookies["Compare"];
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
                    ProductColor productColor = await context.ProductColors.Include(x => x.Product).ThenInclude(x => x.SubCategory).Include(x => x.Product).ThenInclude(x => x.PhoneSpecifications).Include(x=>x.ProductColorImages).Include(x=>x.Discounts).FirstOrDefaultAsync(x => x.Id == item.Id);
                    if (productColor == null) return compareVM;
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

            return compareVM;
        }

        public async Task<CompareVM> CompareNotebookList()
        {
            string compare = httpContext.HttpContext.Request.Cookies["Compare"];
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
                    if (productColor == null) return compareVM;
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


                    if (productColor.Product.SubCategoryId == 17)
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

            return compareVM;
        }

        public async Task<Product> ShowQuickView()
        {
            string productId = httpContext.HttpContext.Request.Cookies["Quickview"];

            Product product;
            if (!string.IsNullOrEmpty(productId))
            {
                int id = Convert.ToInt32(productId);

                product =await context.Products.Include(x=>x.ProductColors).ThenInclude(x=>x.ProductColorImages).Include(x => x.ProductColors).ThenInclude(x => x.Discounts).FirstOrDefaultAsync(x => x.Id == id);

            }
            else
            {
                product = context.Products.Include(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).Include(x => x.ProductColors).ThenInclude(x => x.Discounts).First();
            }
            if (productId == "0")
            {
                product = context.Products.Include(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).Include(x => x.ProductColors).ThenInclude(x => x.Discounts).First();

            }
            return product;
        }

        public async Task<List<Product>> ShopCategory()
        {
            string categoryId = httpContext.HttpContext.Request.Cookies["Category"];
            List<Product> sortedProduct;
            string sortvalue = httpContext.HttpContext.Session.GetString("Sorting");
            string filterstr = httpContext.HttpContext.Session.GetString("Filter");
          

            List<Product> product;
            if (!string.IsNullOrEmpty(categoryId))
            {
                int id = Convert.ToInt32(categoryId);
                product = await context.Products.Include(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).Include(x => x.ProductColors).ThenInclude(x => x.Discounts).Where(x => x.SubCategoryId == id).ToListAsync();

            }
            else
            {
                product = await context.Products.Include(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).Include(x => x.ProductColors).ThenInclude(x => x.Discounts).Where(x => x.SubCategoryId == 1).ToListAsync();
            }
            if (categoryId == "0")
            {
                product = await context.Products.Include(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).Include(x => x.ProductColors).ThenInclude(x => x.Discounts).Where(x => x.SubCategoryId == 1).ToListAsync();

            }

            if (!string.IsNullOrEmpty(sortvalue))
            {
                int value = Convert.ToInt32(sortvalue);
                switch (value)
                {
                    case 4:
                        {
                            sortedProduct = product.OrderByDescending(x => x.ProductColors.First().Price).ToList();
                            break;
                        };
                    case 3:
                        {
                            sortedProduct = product.OrderBy(x => x.ProductColors.First().Price).ToList();
                            break;
                        };
                    default:
                        {
                            sortedProduct = product;
                            break;
                        }

                }
             
            }
            else
            if(sortvalue=="0")
            {
                sortedProduct = product;
            }
            else
            {
                sortedProduct = product;
            }

            if (!string.IsNullOrEmpty(filterstr))
            {
                FilterVM filterVM = JsonConvert.DeserializeObject<FilterVM>(filterstr);
                return sortedProduct.Where(x => x.ProductColors.First().Price >= filterVM.Minimum && x.ProductColors.First().Price <= filterVM.Maximum).ToList();
            }

            return sortedProduct;
        }


    }
}
