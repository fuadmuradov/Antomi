using Antomi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.ViewModel
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Discount> Discounts { get; set; }
        public List<Category> Categories { get; set; }
        public List<HomeCategory> HomeCategories { get; set; }
        public List<ProductVM> ProductVMs { get; set; }
    }
}
