using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Domain.Entities
{
    public class Amenity:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public virtual ICollection<RentalAmenity> RentalAmenities { get; set; } = new List<RentalAmenity>();
    }
}
