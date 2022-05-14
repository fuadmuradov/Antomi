using Antomi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.Property(x => x.Percent).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(false);
            builder.Property(x=>x.StartDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
        }
    }
}
