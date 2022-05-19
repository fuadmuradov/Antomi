using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class SubcategoryToMarka
    {
        public int Id { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        [Required]
        public int MarkaId { get; set; }
        public Marka Marka { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
