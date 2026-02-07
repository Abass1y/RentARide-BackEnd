using Microsoft.EntityFrameworkCore;
using RentARide.Domain;
using RentARide.Domain.Entities;
using RentARide.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentARide.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {   

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
         
        public async Task AddAsync(Users user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
    }
}
