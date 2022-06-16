using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Areas.Admin.Controllers
{
    public class NotificationController : Controller
    {
        private readonly AntomiDbContext context;

        public NotificationController(AntomiDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View(context.Notifications.ToList());
        }

        public IActionResult NotificationDelete(int? id)
        {
            if (id == null) return NotFound();
            Notification notification = context.Notifications.Find(id);
            if (notification == null) return NotFound();
            context.Notifications.Remove(notification);
            context.SaveChanges();
            return LocalRedirect("/Admin/Notification/Index");
        }
    }
}
