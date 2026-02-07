using RentARide.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Domain
{
    public interface IUserRepository
    {
        Task<Users?> GetByEmailAsync(string email);
        Task AddAsync(Users user);
    }
}
