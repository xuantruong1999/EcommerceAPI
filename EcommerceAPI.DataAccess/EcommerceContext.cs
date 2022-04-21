using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EcommerceAPI.DataAccess.EFModel;

namespace EcommerceAPI.DataAccess
{
    public class EcommerceContext : IdentityDbContext<User>
    {
        public EcommerceContext()
        {
        }

        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne<Profile>(user => user.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Profile>()
                .Property(p => p.Version).IsConcurrencyToken();

            builder.Entity<Product>()
                    .HasOne<CategoryProduct>(product => product.CategoryProduct)
                    .WithMany(cate => cate.Products)
                    .HasForeignKey(product => product.CategoryID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
