using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Infrastructure.Configurations
{
    public class VehicleConfigurations : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasIndex(v => v.LicensePlate).IsUnique();
            builder.Property(v => v.LicensePlate).IsRequired().HasMaxLength(20);
            builder.Property(v => v.Model).IsRequired().HasMaxLength(50);

        }
    }
}
