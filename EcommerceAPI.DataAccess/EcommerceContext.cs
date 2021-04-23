using System;
using System.Collections.Generic;
using System.Text;
using EcommerceAPI.DataAccess.Configuration;
using EcommerceAPI.DataAccess.EFModel;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace EcommerceAPI.DataAccess
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext()
        {

        }

        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {
            
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }

        protected void OnModelCreating(ModelBuilder builder)
        {
            builder
               .ApplyConfiguration(new ProfileConfiguration());

            builder
                .ApplyConfiguration(new UserConfiguration());

            builder.Entity<User>()
                .HasKey(u => u.Id);
            
            builder.Entity<Profile>()
                .HasKey(p => p.Id);
        }
    }
}
