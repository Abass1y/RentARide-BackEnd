using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Domain.Entities
{
    public class RentalAmenity
    {
        public Guid RentalId { get; set; }
        public virtual Rental Rental { get; set; } = null!;
        
        public Guid AmenityId { get; set; }
        public virtual Amenity Amenity { get; set; } = null!;
    }
}
