using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int ProductColorId { get; set; }
        public ProductColor ProductColor { get; set; }
        public double Price { get; set; }
    }
}
