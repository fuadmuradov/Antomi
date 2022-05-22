using Antomi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.ViewModel
{
    public class BasketItemVM
    {
        public ProductColor ProductColor { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
    }
}
