using Microsoft.EntityFrameworkCore;
using RentARide.Application.Interfaces;
using RentARide.Domain.Entities;
using RentARide.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .Where(v=> !v.IsDeleted)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(Guid Id)
        {
            return await _context.Vehicles.FindAsync(Id);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            if (vehicle.Id == Guid.Empty)
                vehicle.Id = Guid.NewGuid();

             await _context.Vehicles.AddAsync(vehicle);
             await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
                  _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid Id)
        {
            var vehicle = await _context.Vehicles.FindAsync(Id);

            if (vehicle != null)
            {
               _context.Vehicles.Remove(vehicle);
              await _context.SaveChangesAsync();
            }
        }
    }
}
