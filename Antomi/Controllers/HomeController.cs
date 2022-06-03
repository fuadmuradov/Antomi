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
using Microsoft.AspNetCore.Identity;

namespace Antomi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AntomiDbContext context;
        private readonly UserManager<AppUser> userManager;

        public HomeController(AntomiDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<Discount> discounts = await context.Discounts.Include(x => x.ProductColor).ThenInclude(x => x.ProductColorImages).Include(x => x.ProductColor).ThenInclude(x => x.Product).Where(x => x.DealofMonth == true).ToListAsync();
            List<Category> categories = context.Categories.Include(x => x.SubCategories).ToList();
            List<HomeCategory> homeCategories = context.HomeCategories.Include(x => x.Category).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Products).ThenInclude(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).Include(x => x.Category).ThenInclude(x => x.SubCategories).ThenInclude(x => x.Products).ThenInclude(x => x.ProductColors).ThenInclude(x => x.Discounts).ToList();
            List<ProductVM> productVMs = new List<ProductVM>();
            List<Product> products;
            foreach (var item in context.HomeCategories.Include(x=>x.Category).ThenInclude(x=>x.SubCategories).ToList())
            {
                products = context.Products.Include(x => x.ProductColors).ThenInclude(x => x.ProductColorImages).Include(x => x.ProductColors).ThenInclude(x => x.Discounts).Where(x => x.SubCategory.CategoryId == item.CategoryId).OrderByDescending(x => x.ProductColors.First().Price).Take(12).ToList();
                ProductVM productVM = new ProductVM()
                {
                    HomeCategory = item,
                    Products = products
                };
                productVMs.Add(productVM);
            }

            
            HomeVM homeVM = new HomeVM()
            {
                Sliders = context.Sliders.ToList(),
                Discounts = discounts,
                Categories = categories,
                HomeCategories = homeCategories,
                ProductVMs = productVMs
            };
            return View(homeVM);
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
            if (product == null) return NotFound();
            List<SubcategoryToMarka> subcategoryToMarka = context.SubcategoryToMarkas.Include(x=>x.SubCategory).Include(x=>x.Marka).Where(x=>x.MarkaId==product.MarkaId && x.SubCategoryId==product.SubCategoryId).ToList();
            Marka marka = context.Markas.Include(x=>x.Products).ThenInclude(x=>x.ProductColors).ThenInclude(x=>x.ProductColorImages).Include(x=>x.SubcategoryToMarkas).Include(x => x.Products).ThenInclude(x => x.ProductColors).ThenInclude(x=>x.Discounts).FirstOrDefault(x => x.Id == product.MarkaId);
            List<Product> products = marka.Products.Where(x => x.SubCategoryId == product.SubCategoryId).ToList();
            ViewBag.RelatedProduct = products;
            ViewBag.Comments = context.Comments.OrderBy(x=>x.CretadAt).Where(x => x.ProductId == id && x.isActive).ToList();
            
            return View(product);
        }

        public async Task<IActionResult> AddComment(string comment, string comName, string comEmail, int comProductId)
        {

            Comment ccomment;
            if (User.Identity.IsAuthenticated)
            {
                ccomment = new Comment()
                {
                    Text = comment,
                    Username = comName,
                    Email = comEmail,
                    ProductId = comProductId,
                    isActive = false,
                    AppUserId = User.Identity.Name,
                    CretadAt = DateTime.UtcNow.AddHours(+4)
                };
            }
            else
            {
                ccomment = new Comment()
                {
                    Text = comment,
                    Username = comName,
                    Email = comEmail,
                    ProductId = comProductId,
                    isActive = false,
                    CretadAt = DateTime.UtcNow
                };
            }

            await context.Comments.AddAsync(ccomment);
            await context.SaveChangesAsync();



            return RedirectToAction("Details", "Home", new { id = comProductId }) ;
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
        public async Task<IActionResult> AddtoBasket(int ColorId, int Quantity)
        {
            ProductColor productColor = context.ProductColors.FirstOrDefault(x => x.Id == ColorId && x.Count >= Quantity);
            if (productColor == null) return NotFound();

            if (!User.Identity.IsAuthenticated)
            {
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
            }
            else
            {
                AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
                Cart cart = await context.Carts.FirstOrDefaultAsync(x => x.ProductColorId == ColorId);
                if(cart == null)
                {
                    Cart newcart = new Cart()
                    {
                        AppUserId = user.Id,
                        ProductColorId = ColorId,
                        Quantity = Quantity,
                        Price = productColor.Price
                    };
                    await context.Carts.AddAsync(newcart);
                 
                }
                else
                {
                    cart.Quantity++;
                }
                await context.SaveChangesAsync();
            }
            return PartialView("_CartPartialView");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBasketItem(int itemID)
        {
            if (!User.Identity.IsAuthenticated)
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
            }
            else
            {
                AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
                Cart cart = await context.Carts.FirstOrDefaultAsync(x => x.ProductColorId == itemID);
                if(cart != null)
                {
                    context.Carts.Remove(cart);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return PartialView("_CartPartialView");
                }
         
            }


            return PartialView("_CartPartialView");
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

        public IActionResult Search(int select, string searchInput)
        {

            SearchVM searchVM = new SearchVM()
            {
                SubCategoryId = select,
                SearchText = searchInput
            };

            string searchstr = JsonConvert.SerializeObject(searchVM, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            HttpContext.Response.Cookies.Append("Search", searchstr);
            return RedirectToAction(nameof(Shop), "Home");
        }


        #endregion
    }
}
