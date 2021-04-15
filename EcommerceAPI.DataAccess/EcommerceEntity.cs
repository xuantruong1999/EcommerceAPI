using System;
using System.Collections.Generic;
using System.Text;
using EcommerceAPI.DataAccess.Configuration;
using EcommerceAPI.DataAccess.EFModel;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.DataAccess
{
    public class EcommerceEntity : DbContext
    {
        public EcommerceEntity(DbContextOptions<EcommerceEntity> options) : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }

        protected void OnModelCreating(ModelBuilder builder)
        {

            //builder.Configurations.Add(new UserConfiguration());

            //builder
            //   .ApplyConfiguration(new ProfileConfiguration());

            builder.Entity<User>()
                .HasKey(u => u.Id);
            


            builder.Entity<Profile>()
                .HasKey(p => p.Id);

        }
    }
}
