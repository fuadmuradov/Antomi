using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class About
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Signature { get; set; }
        public IFormFile SignPhoto { get; set; }
    }
}
