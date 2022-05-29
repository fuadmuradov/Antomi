using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    public class CommentController : Controller
    {
        private readonly AntomiDbContext context;

        public CommentController(AntomiDbContext context)
        {
            this.context = context;
        }
        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
           // List<Comment> comments
            List<Comment> comments = context.Comments.Include(x=>x.Product).Where(x => x.ProductId == id).ToList();

            return View(comments);
        }

        public async Task<IActionResult> ChangeCommentStatus(int id)
        {
            Comment comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null) return NotFound();
            if (comment.isActive)
            {
                comment.isActive = false;
            }
            else
            {
                comment.isActive = true;
            }
            await context.SaveChangesAsync();

            return LocalRedirect("/Admin/Comment/Index/"+comment.ProductId.ToString());
        }

    }
}
