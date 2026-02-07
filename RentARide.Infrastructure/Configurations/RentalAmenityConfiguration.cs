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
     public class RentalAmenityConfiguration:IEntityTypeConfiguration<RentalAmenity>
    {
        public void Configure(EntityTypeBuilder<RentalAmenity> builder)
        {
            builder.HasKey(rm => new { rm.RentalId, rm.AmenityId });

        }
    }
}
