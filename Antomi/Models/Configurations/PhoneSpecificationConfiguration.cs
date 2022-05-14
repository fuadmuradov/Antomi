using Antomi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Configurations
{
    public class PhoneSpecificationConfiguration : IEntityTypeConfiguration<PhoneSpecification>
    {
        public void Configure(EntityTypeBuilder<PhoneSpecification> builder)
        {
            builder.Property(x => x.RAM).IsRequired();
            builder.Property(x => x.Processor).IsRequired();
            builder.Property(x => x.OS).IsRequired();
            builder.Property(x => x.SelfieCamera).IsRequired();
            builder.Property(x => x.MainCamera).IsRequired();
            builder.Property(x => x.Storage).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();

        }
    }
}
