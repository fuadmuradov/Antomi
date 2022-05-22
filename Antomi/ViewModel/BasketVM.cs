using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.ViewModel
{
    public class BasketVM
    {
        public List<BasketItemVM> BasketItems { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
    }
}
