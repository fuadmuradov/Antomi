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
    public class SettingController : Controller
    {
        private readonly AntomiDbContext context;
        private readonly IWebHostEnvironment webHost;

        public SettingController(AntomiDbContext context, IWebHostEnvironment webHost)
        {
            this.context = context;
            this.webHost = webHost;
        }
        public IActionResult Index()
        {
            Setting setting = context.Settings.First();
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }

            Setting existSetting = context.Settings.FirstOrDefault(x => x.Id == setting.Id);

            if (setting.Photo != null)
            {
                try
                {
                    string folder = @"assets\img\logo\";
                    string newImg = await setting.Photo.SavaAsync(webHost.WebRootPath, folder);
                    FileExtension.Delete(webHost.WebRootPath, folder, existSetting.LogoImage);
                    existSetting.LogoImage = newImg;
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "unexpected error for Update");
                    return View();
                }
            }

            existSetting.Email = setting.Email;
            existSetting.Phone = setting.Phone;
            existSetting.Address = setting.Address;
            existSetting.Description = setting.Description;
            existSetting.FacebookUrl = setting.FacebookUrl;
            existSetting.TwitterUrl = setting.TwitterUrl;
            existSetting.LinkedinUrl = setting.LinkedinUrl;
            existSetting.InstagramUrl = setting.InstagramUrl;
            existSetting.PlaystoreUrl = setting.PlaystoreUrl;
            existSetting.AppstoreUrl = setting.AppstoreUrl;


            await context.SaveChangesAsync();


            return View(setting);
        }
    }
}
