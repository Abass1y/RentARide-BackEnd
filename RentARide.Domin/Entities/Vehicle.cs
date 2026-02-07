using RentARide.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Domain.Entities
{
    public class Vehicle:BaseEntity
    {

        public string Model { get; set; } = string.Empty;
        public DateTime Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public VehicleStatus Status { get; set; }


        public Guid VehicleTypeId { get; set; }
        public virtual VehicleType VehicleType { get; set; } = null!;
        public virtual VehicleMaintenance? Maintenance { get; set; }
    }
}
