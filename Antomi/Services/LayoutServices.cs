using Antomi.DataAccsessLayer;
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

    }
}
