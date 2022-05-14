using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class ProductColor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public int PrpductId { get; set; }
        public Product Product { get; set; }
        public List<ProductColorImage> ProductColorImages { get; set; }
    }
}
