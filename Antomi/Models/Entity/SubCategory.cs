using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class SubCategory:BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
        public List<SubcategoryToMarka> SubcategoryToMarkas { get; set; }


    }

    public class SubCategoryValidation : AbstractValidator<SubCategory>
    {
        public SubCategoryValidation()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Category Name Must be Filled")
                .MaximumLength(150).WithMessage("Group Name character's maximum length 150");
        }
    }
}
