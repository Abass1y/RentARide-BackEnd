using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Application.DTOs
{
    public class UpdateVehicleDto
    {
        public string Model { get; set; } = string.Empty;
        public DateTime Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public decimal DailyPrice { get; set; }
        public Guid VehicleTypeId { get; set; }
    }
}
