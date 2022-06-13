using Antomi.DataAccsessLayer;
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
    }
}
