using Antomi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.ViewModel
{
    public class CheckoutVM
    {
        public BasketVM BasketVM { get; set; }
        public Address Address { get; set; }
    }
}
