using Antomi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.ViewModel
{
    public class ProductVM
    {
        public HomeCategory HomeCategory { get; set; }
        public List<Product> Products { get; set; }
    }
}
