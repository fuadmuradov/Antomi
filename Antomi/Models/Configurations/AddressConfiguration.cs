using Antomi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(70);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(70);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(90);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(70);
            builder.Property(x => x.Country).IsRequired().HasMaxLength(70);
            builder.Property(x => x.Town).IsRequired().HasMaxLength(70);
            builder.Property(x => x.Street).IsRequired().HasMaxLength(300);

        }
    }
}
