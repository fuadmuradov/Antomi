using Antomi.DataAccsessLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antomi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Antomi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly AntomiDbContext context;

        public HomeController(AntomiDbContext context)
        {
            this.context = context;
        }
        //******************************
        #region Category CRUD
        public IActionResult Category()
        {
            List<Antomi.Models.Entity.Category> categories = context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Category(string cName)
        {
            if (String.IsNullOrEmpty(cName)) return BadRequest();

            Category category = new Category()
            {
                Name = cName
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            List<Antomi.Models.Entity.Category> categories = context.Categories.ToList();
            return View(categories);
        }
        [HttpGet("{id}")]
        public async Task<JsonResult> EditCategory(int? id)
        {
            if (id == null)
            {
                return Json(new
                {
                    status = 404
                });
            }

            var ctgr = await context.Categories.FindAsync(id);
            if (ctgr == null)
            {
                return Json(new
                {
                    status = 404
                });
            }

            return Json(ctgr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditCategory(int id, string name)
        {
            Category category = await context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (category == null)
            {
                return Json(new
                {
                    status = 404
                });
            }
            category.Name = name;
            category.ModifiedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return Json(new
            {
                status = 200
            });
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) return NotFound();
            List<SubCategory> subCategories = await context.SubCategories.Where(x => x.CategoryId == id).ToListAsync();
            if (subCategories.Count != 0)
                context.SubCategories.RemoveRange(subCategories);

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return LocalRedirect("/admin/Home/Category");
        }

        #endregion

        //******************************

        #region SubCategory
        public IActionResult SubCategory()
        {
            ViewBag.Category = context.Categories.ToList();

            List<SubCategory> subCategories = context.SubCategories.Include(x => x.Category).ToList();

            return View(subCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subcategory(int SId, string Name)
        {
            ViewBag.Category = context.Categories.ToList();
            Category category = context.Categories.FirstOrDefault(x => x.Id == SId);
            if (category == null) return NotFound();
            if (Name == null) return BadRequest();

            Models.Entity.SubCategory subcategory = new SubCategory()
            {
                CategoryId = SId,
                Name = Name
            };

            context.SubCategories.Add(subcategory);
            await context.SaveChangesAsync();

            return LocalRedirect("/admin/home/subcategory/");
        }


        [HttpGet("{id}")]
        public async Task<JsonResult> EditSubCategory(int? id)
        {
            if (id == null)
            {
                return Json(new
                {
                    status = 404
                });
            }

            var sctgr = await context.SubCategories.FindAsync(id);
            if (sctgr == null)
            {
                return Json(new
                {
                    status = 404
                });
            }

            return Json(sctgr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditSubCategory(int id, int CId, string name)
        {
            SubCategory subCategory = await context.SubCategories.FirstOrDefaultAsync(p => p.Id == id);
            if (subCategory == null)
            {
                return Json(new
                {
                    status = 404
                });
            }
            subCategory.Name = name;
            subCategory.CategoryId = CId;
            subCategory.ModifiedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return Json(new
            {
                status = 200
            });
        }

        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            SubCategory subCategory = context.SubCategories.FirstOrDefault(x => x.Id == id);
            if (subCategory == null) return NotFound();
            context.SubCategories.Remove(subCategory);
            await context.SaveChangesAsync();

            return LocalRedirect("/admin/home/subcategory/");
        }


        #endregion

        //**********************************
        #region Marka
        public IActionResult Marka()
        {
            ViewBag.SubCategory = context.SubCategories.ToList();
            List<Marka> markas = context.Markas.Include(x => x.SubcategoryToMarkas).ToList();
            return View(markas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Marka(List<int> SubIds, string Name)
        {
            ViewBag.SubCategory = context.SubCategories.ToList();

            if (Name == null) return BadRequest();

            Models.Entity.Marka marka = new Marka()
            {
                Name = Name
            };

            context.Markas.Add(marka);
            await context.SaveChangesAsync();

            foreach (var item in SubIds)
            {
                SubcategoryToMarka subcategoryToMarka = new SubcategoryToMarka()
                {
                    SubCategoryId = item,
                    MarkaId = marka.Id
                };
                context.SubcategoryToMarkas.Add(subcategoryToMarka);
                await context.SaveChangesAsync();
            }


            return LocalRedirect("/admin/home/Marka/");
        }



        [HttpGet("{id}")]
        public async Task<JsonResult> EditMarka(int? id)
        {
           

            if (id == null)
            {
                return Json(new
                {
                    status = 404
                });
            }
            var marka = context.Markas.Include(x => x.SubcategoryToMarkas).FirstOrDefault(x => x.Id == id);
            if (marka == null)
            {
                return Json(new
                {
                    status = 404
                });
            }

            return Json(marka);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EditMarka(int id, List<int> SId, string name)
        {
            Marka marka = await context.Markas.FirstOrDefaultAsync(p => p.Id == id);
            if (marka == null)
            {
                return Json(new
                {
                    status = 404
                });
            }
            marka.Name = name;
            marka.ModifiedAt = DateTime.UtcNow;

            List<SubcategoryToMarka> toMarkas = context.SubcategoryToMarkas.Where(x => x.MarkaId == id).ToList();
            if (toMarkas.Count != 0)
                context.SubcategoryToMarkas.RemoveRange(toMarkas);
            foreach (var item in SId)
            {
                SubcategoryToMarka subcategoryToMarka = new SubcategoryToMarka()
                {
                    SubCategoryId = item,
                    MarkaId = marka.Id
                };
                context.SubcategoryToMarkas.Add(subcategoryToMarka);
                await context.SaveChangesAsync();
            }
            await context.SaveChangesAsync();
            return Json(new
            {
                status = 200
            });
        }


        public async Task<IActionResult> DeleteMarka(int id)
        {
            Marka marka = context.Markas.FirstOrDefault(x => x.Id == id);
            if (marka == null) return NotFound();
            context.Markas.Remove(marka);
            await context.SaveChangesAsync();

            return LocalRedirect("/admin/home/marka/");
        }
        #endregion


    }
}
