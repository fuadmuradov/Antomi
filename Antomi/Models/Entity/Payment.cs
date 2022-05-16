using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Payment:BaseEntity
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Amount { get; set; }
        public bool Success { get; set; }
    }
}
