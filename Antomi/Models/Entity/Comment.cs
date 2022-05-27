using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public int Text { get; set; }
        public int ProductId { get; set; }
        public bool isActive { get; set; }
        public Product Product { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }


    }
}
