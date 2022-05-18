using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.ViewModel
{
    public class ColorImageVM
    {
        [Required]
        [StringLength(maximumLength:50)]
        public string ColorName { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public IFormFileCollection ColorImages { get; set; }



    }
}
