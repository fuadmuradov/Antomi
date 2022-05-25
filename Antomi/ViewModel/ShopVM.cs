using Antomi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.ViewModel
{
    public class ShopVM
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public CompareVM CompareVM { get; set; }
    }
}
