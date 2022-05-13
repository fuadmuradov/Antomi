using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
    }

    public class CategoryValidation : AbstractValidator<Category>
    {
        public CategoryValidation()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Category Name Must be Filled")
                .MaximumLength(150).WithMessage("Group Name character's maximum length 150");
        }
    }
}
