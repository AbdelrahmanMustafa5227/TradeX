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

        public async Task<List<Subscription>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Subscription?> GetByUserIdAsync(Guid userId)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public void ZeriongAll()
        {
             DbSet.ExecuteUpdate(p => p.SetProperty(x => x.ComulativeTradingVolume24H , 0));
        }
    }
}
