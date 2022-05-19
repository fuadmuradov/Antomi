using Antomi.DataAccsessLayer;
using Antomi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AntomiDbContext context;

        public HomeController(AntomiDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

    }
}
