using Bookify.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Users;

namespace TradeX.Infrastructure.Persistance.Repositories
{
    internal class UserRepository : Repository<User> , IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext): base(dbContext)
        {

        }

        public async Task<bool> IsEmailUnique(string email)
        {
            return !await DbSet.AnyAsync(x => x.Email == email);
        }
    }
}
