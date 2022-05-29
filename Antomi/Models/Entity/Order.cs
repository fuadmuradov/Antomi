using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Order:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public double TotalPrice { get; set; }
    }
}
