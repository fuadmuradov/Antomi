using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class ProductColorImage:BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public int ProductColorId { get; set; }
        public ProductColor ProductColor { get; set; }
    }
}
