using Bookify.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;

namespace TradeX.Infrastructure.Persistance.Repositories
{
    internal class FutureOrderRepository : Repository<FutureOrder> , IFutureOrderRepository
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

        public Task<List<FutureOrder>> GetAllOpenOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<FutureOrder>> GetOpenByCryptoIdAsync(Guid cryptoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FutureOrder>> GetOpenByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
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
