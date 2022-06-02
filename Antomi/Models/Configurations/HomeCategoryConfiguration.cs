using Antomi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Configurations
{
    public class HomeCategoryConfiguration : IEntityTypeConfiguration<HomeCategory>
    {
        public void Configure(EntityTypeBuilder<HomeCategory> builder)
        {
            builder.Property(x => x.Image).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
               
        }
    }
}