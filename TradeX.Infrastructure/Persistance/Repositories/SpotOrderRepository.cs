using Bookify.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.SpotOrders;

namespace TradeX.Infrastructure.Persistance.Repositories
{
    internal class SpotOrderRepository : Repository<SpotOrder> , ISpotOrderRepository
    {
        public SpotOrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }


        public Task<List<SpotOrder>> GetAllByCryptoIdAsync(Guid cryptoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SpotOrder>> GetAllByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SpotOrder>> GetAllOpenOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SpotOrder>> GetOpenByCryptoIdAsync(Guid cryptoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SpotOrder>> GetOpenByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void Remove(SpotOrder order)
        {
            DbSet.Remove(order);
        }

        public Task Update(SpotOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
