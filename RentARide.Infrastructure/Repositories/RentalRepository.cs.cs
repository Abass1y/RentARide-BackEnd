using Microsoft.EntityFrameworkCore;
using RentARide.Domain.Entities;
using RentARide.Domain.interfaces;
using RentARide.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Infrastructure.Repositories
{
    public class RentalRepository(ApplicationDbContext context) : IRentalRepository
    {
        public async Task AddAsync(Rental rental)
        {
            await context.Rentals.AddAsync(rental);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsByVehicleAsync(Guid vehicleId)
        {
           
            return await context.Rentals
                .Where(r => r.VehicleId == vehicleId && r.Status == "Active")
                .ToListAsync();
        }
    }
}
