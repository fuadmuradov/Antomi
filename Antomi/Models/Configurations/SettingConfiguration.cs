using Antomi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(x => x.LogoImage).IsRequired();
            builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(150);
            builder.Property(x => x.FacebookUrl).IsRequired();
            builder.Property(x => x.TwitterUrl).IsRequired();
            builder.Property(x => x.InstagramUrl).IsRequired();
            builder.Property(x => x.LinkedinUrl).IsRequired();
            builder.Property(x => x.PlaystoreUrl).IsRequired();
            builder.Property(x => x.AppstoreUrl).IsRequired();
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);


        }
    }

}
