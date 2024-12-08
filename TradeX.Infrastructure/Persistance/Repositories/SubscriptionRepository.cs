using Bookify.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Subscriptions;

namespace TradeX.Infrastructure.Persistance.Repositories
{
    internal class SubscriptionRepository : Repository<Subscription> , ISubscriptionRepository
    {
        public SubscriptionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public Task<List<Subscription>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Subscription?> GetByUserIdAsync(Guid userId)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
