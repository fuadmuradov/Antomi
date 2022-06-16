using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Antomi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Controllers
{
    public class AboutController : Controller
    {
        private readonly AntomiDbContext context;

        public AboutController(AntomiDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            AboutVM aboutVM = new AboutVM()
            {
                About = context.Abouts.First(),
                Testimonials = context.Testimonials.ToList(),
                Questions = context.Questions.ToList()
            };
            return View(aboutVM);
        }

        public IActionResult Contact()
        {
            Setting setting = context.Settings.First();
            return View(setting);
        }

        public IActionResult SendMail(string name, string email, string subject, string message)
        {
            Notification notification = new Notification()
            {
                Name = name,
                Email = email,
                Subject = subject,
                Message = message
            };

            context.Notifications.Add(notification);
            context.SaveChanges();
            return RedirectToAction(nameof(Contact), "About");
        }
             
    }
}
