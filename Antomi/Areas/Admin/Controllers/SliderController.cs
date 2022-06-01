using Antomi.DataAccsessLayer;
using Antomi.Extension;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class SliderController : Controller
    {
        private readonly AntomiDbContext context;
        private readonly IWebHostEnvironment webHost;

        public SliderController(AntomiDbContext context, IWebHostEnvironment webHost)
        {
            this.context = context;
            this.webHost = webHost;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = context.Sliders.ToList();

            return View(sliders);
        }

        public IActionResult AddSlider()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSlider(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("ColorImages", "image type is not Correct");
                return View();
            }
            string folder = @"assets\img\slider\";
            slider.Image = slider.Photo.SavaAsync(webHost.WebRootPath, folder).Result;

           await context.Sliders.AddAsync(slider);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Slider");
        }

        public IActionResult UpdateSlider(int id)
        {
            Slider slider = context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null) return NotFound();

            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSlider(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            Slider existSlider = context.Sliders.FirstOrDefault(x => x.Id == slider.Id);

            if (slider.Photo != null)
            {
                try
                {
                    string folder = @"assets\img\slider\";
                    string newImg = await slider.Photo.SavaAsync(webHost.WebRootPath, folder);
                    FileExtension.Delete(webHost.WebRootPath, folder, existSlider.Image);
                    existSlider.Image = newImg;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "unexpected error for Update");
                    return View();
                }

            }

            existSlider.Title = slider.Title;
            existSlider.Discount = slider.Discount;
            
            await context.SaveChangesAsync();


            return RedirectToAction(nameof(Index), "Slider");
        }

        public IActionResult DeleteSlider(int id)
        {
            Slider existSlider = context.Sliders.FirstOrDefault(x => x.Id == id);

            if (existSlider == null) return NotFound();
            context.Sliders.Remove(existSlider);
            context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Slider");
        }




        }
}
