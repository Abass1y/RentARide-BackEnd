using RentARide.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Domain.interfaces
{
    public interface IRentalRepository
    {
        Task AddAsync(Rental rental);
        Task<IEnumerable<Rental>> GetActiveRentalsByVehicleAsync(Guid vehicleId);
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(Guid userId);
    }
}
