using Antomi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.ViewModel
{
    public class AboutVM
    {
        public About  About { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<Question> Questions { get; set; }
    }
}
