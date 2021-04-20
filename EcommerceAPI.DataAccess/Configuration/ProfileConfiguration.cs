using EcommerceAPI.DataAccess.EFModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess.Configuration
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder
                .HasKey(k => k.Id);

            builder
                .Property(k => k.Address).HasMaxLength(300);

            builder
                .HasOne(p => p.OwnUser)
                .WithOne(u => u.Profile);
                
        }

    }
}
