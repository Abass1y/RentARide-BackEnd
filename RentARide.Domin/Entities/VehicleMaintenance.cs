using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Domain.Entities
{
    public class VehicleMaintenance:BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public DateTime LastMaintenanceDate { get; set; }

        public Guid VehicleId { get; set; } // Forgin Key
        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
