using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class HomeCategory
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class HomeCategoryValidation : AbstractValidator<HomeCategory>
    {
        public HomeCategoryValidation()
        {
            RuleFor(x => x.Photo).NotEmpty().NotNull();
            RuleFor(x => x.CategoryId).NotEmpty().NotNull();
           
        }
    }
}
