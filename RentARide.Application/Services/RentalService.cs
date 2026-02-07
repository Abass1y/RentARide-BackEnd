using RentARide.Application.Interfaces;
using RentARide.Domain.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Application.Services
{
    public class RentalService(IRentalRepository _rentalRepository, IVehicleRepository _vehicleRepository)
    {
        public async Task<decimal> CalculateTotalPrice(Guid vehicleId, DateTime start, DateTime end)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null) throw new Exception("Vehicle not found");

            
            int days = (end - start).Days;
            if (days <= 0) days = 1; 

            return days * vehicle.PricePerDay;
        }

        public async Task<bool> IsVehicleAvailable(Guid vehicleId, DateTime start, DateTime end)
        {
            var existingRentals = await _rentalRepository.GetActiveRentalsByVehicleAsync(vehicleId);

            return !existingRentals.Any(r =>
                (start < r.EndDate && end > r.StartDate));
        }
    }
}
