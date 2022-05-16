using Antomi.Models.Configurations;
using Antomi.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antomi.DataAccsessLayer
{
    public class AntomiDbContext : IdentityDbContext<AppUser>
    {
        public AntomiDbContext(DbContextOptions<AntomiDbContext> options) : base(options) { }
        public DbSet<AppUser> AppUsers {get; set;}
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Marka> Markas { get; set; }
        public DbSet<NotebookSpecification> NotebookSpecifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PhoneSpecification> PhoneSpecifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductColorImage> ProductColorImages { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());
            modelBuilder.ApplyConfiguration(new MarkaConfiguration());
            modelBuilder.ApplyConfiguration(new NotebookSpecificationConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneSpecificationConfiguration());
            modelBuilder.ApplyConfiguration(new ProductColorImageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductColorConfiguration());
            modelBuilder.ApplyConfiguration(new SpecificationConfiguration());
            modelBuilder.ApplyConfiguration(new SubCategoryConfiguration());

            modelBuilder.Entity<Marka>()
               .HasMany(c => c.Products)
               .WithOne(e => e.Marka)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Payment>()
                .HasOne(c => c.Order)
                .WithOne(c => c.Payment)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.OnModelCreating(modelBuilder);

        }
    }
}
