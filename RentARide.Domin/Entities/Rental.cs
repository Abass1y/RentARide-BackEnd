using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Domain.Entities
{
    public class Rental:BaseEntity
    {
        public Guid VehicleId { get; set; } 
        public virtual Vehicle Vehicle { get; set; } = null!;

        public Guid UserId { get; set; } 
        public virtual Users User { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Active";
    }
}
