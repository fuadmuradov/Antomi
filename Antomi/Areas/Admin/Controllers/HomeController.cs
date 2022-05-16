using Antomi.DataAccsessLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View();
        }

        #endregion

    }
}
