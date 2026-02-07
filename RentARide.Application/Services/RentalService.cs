using RentARide.Application.Interfaces;
using RentARide.Domain.interfaces;
using System.Net.Http.Json;

namespace RentARide.Application.Services
{
    public class RentalService(
        IRentalRepository _rentalRepository,
        IVehicleRepository _vehicleRepository,
        IHttpClientFactory _httpClientFactory)
    {
        public async Task<decimal> CalculateTotalPrice(Guid vehicleId, DateTime startDate, DateTime endDate)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null) throw new Exception("Vehicle not found");

            int days = (endDate - startDate).Days;
            if (days <= 0) days = 1;

          
            decimal basePrice = days * vehicle.PricePerDay;

            try
            {
              
                var client = _httpClientFactory.CreateClient();
                var holidays = await client.GetFromJsonAsync<List<PublicHoliday>>(
                    $"https://date.nager.at/api/v3/PublicHolidays/{startDate.Year}/DE");

               
                if (holidays?.Any(h => h.Date.Date == startDate.Date) ?? false)
                {
                    basePrice *= 1.10m; 
                }
            }
            catch (Exception)
            {
             
            }

            return basePrice;
        }

        public async Task<bool> IsVehicleAvailable(Guid vehicleId, DateTime start, DateTime end)
        {
            var existingRentals = await _rentalRepository.GetActiveRentalsByVehicleAsync(vehicleId);

            return !existingRentals.Any(r => (start < r.EndDate && end > r.StartDate));
        }
        public async Task CheckOverdueRentals()
        {
           
            var overdueRentals = await _rentalRepository.GetActiveOverdueRentalsAsync();

            foreach (var rental in overdueRentals)
            {
                
                Console.WriteLine($"[WARNING]: {DateTime.UtcNow} - Rental {rental.Id} is overdue. User {rental.UserId} has not returned the car.");
            }
        }
    }

    
    public record PublicHoliday(DateTime Date, string Name);
}