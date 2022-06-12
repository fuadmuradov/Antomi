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
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PhoneSpecification> PhoneSpecifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductColorImage> ProductColorImages { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<SubcategoryToMarka> SubcategoryToMarkas { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<HomeCategory> HomeCategories { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<ReplyComment> ReplyComments { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<About> Abouts { get; set; }


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
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new WishlistConfiguration());
            modelBuilder.ApplyConfiguration(new SliderConfiguration());
            modelBuilder.ApplyConfiguration(new BlogConfiguration());
            modelBuilder.ApplyConfiguration(new HomeCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new BlogCommentConfiguration());
            modelBuilder.ApplyConfiguration(new ReplyCommentConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new TestimonialConfiguration());
            modelBuilder.ApplyConfiguration(new AboutConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());

            modelBuilder.Entity<Marka>()
               .HasMany(c => c.Products)
               .WithOne(e => e.Marka)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<SubCategory>()
           .HasMany(c => c.Products)
           .WithOne(e => e.SubCategory)
           .OnDelete(DeleteBehavior.ClientSetNull);

        //    modelBuilder.Entity<AppUser>()
        //  .HasMany(c => c.BlogComments)
        //  .WithOne(e => e.AppUser)
        //  .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<Blog>()
        //  .HasMany(c => c.BlogComments)
        //  .WithOne(e => e.Blog)
        //  .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<AppUser>()
        // .HasMany(c => c.ReplyComments)
        // .WithOne(e => e.AppUser)
        // .OnDelete(DeleteBehavior.Restrict);

        //    modelBuilder.Entity<BlogComment>()
        //.HasMany(c => c.ReplyComments)
        //.WithOne(e => e.BlogComment)
        //.OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);

        }
    }
}
