using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Entity
{
    public class Product:BaseEntity
    {
        public string Model { get; set; }
        public string Description { get; set; }
        public int MarkaId { get; set; }
        public Marka Marka { get; set; }

        public int? SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public List<Specification> Specifications { get; set; }
        public List<Comment> Comments { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<PhoneSpecification> PhoneSpecifications { get; set; }
        public List<NotebookSpecification> NotebookSpecifications { get; set; }
    }   
}
