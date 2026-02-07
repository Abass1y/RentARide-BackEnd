using Microsoft.EntityFrameworkCore;
using RentARide.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Infrastructure.Persistence
{
    public class ApplicationDbContext:DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        { AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Rental>Rentals { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<RentalAmenity> RentalAmenities { get; set; }
        public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
