using Bookify.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;

namespace TradeX.Infrastructure.Persistance.Repositories
{
    internal class FutureOrderRepository : Repository<FutureOrder>, IFutureOrderRepository
    {
        public FutureOrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public Task<List<FutureOrder>> GetAllByCryptoIdAsync(Guid cryptoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FutureOrder>> GetAllByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FutureOrder>> GetAllLimitOrdersAsync()
        {
            return await DbSet.Where(x => x.OpenedOnUtc == null).ToListAsync();
        }

        public async Task<List<FutureOrder>> GetAllOpenOrdersAsync()
        {
            return await DbSet.Where(x => x.IsActive == true &&
                                    (x.TakeProfitPrice != null || x.StopLossPrice != null))
                              .ToListAsync();
        }

        public void Remove(FutureOrder order)
        {
            DbSet.Remove(order);
        }

        public Task Update(FutureOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
