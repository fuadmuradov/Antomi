using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Discount
    {
        public int Id { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProductColorId { get; set; }
        public ProductColor ProductColor { get; set; }
    }
}
