using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Order:BaseEntity
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public double TotalPrice { get; set; }
        public double TotalShipping { get; set; }
    }
}
