using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentARide.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Infrastructure.Configurations
{
    public class UserConfigurations:IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(25);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(25);
         
        }
    }
}
