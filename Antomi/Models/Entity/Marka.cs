using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Marka:BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        [NotMapped]
        public List<int> SubCategoryIds { get; set; }
        public List<SubcategoryToMarka> SubcategoryToMarkas { get; set; }
    }
}
