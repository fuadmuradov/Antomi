using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Blog:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string Emphasis { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<BlogComment> BlogComments { get; set; }
    }

    public class BlogValidation : AbstractValidator<Blog>
    {
        public BlogValidation()
        {
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
            RuleFor(x => x.Title).NotEmpty().NotNull().MaximumLength(150);
            RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(3000);
            RuleFor(x => x.Emphasis).MaximumLength(500);
        }
    }
}
