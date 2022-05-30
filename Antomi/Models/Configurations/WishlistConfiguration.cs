using Antomi.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.Models.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.Property(x => x.AppUserId).IsRequired();
            builder.Property(x => x.ProductColorId).IsRequired();
            builder.Property(x => x.Price).IsRequired();
           

        }
    }
}
