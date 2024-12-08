using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;

namespace TradeX.Domain.SpotOrders
{
    public interface ISpotOrderRepository
    {
        void Add(SpotOrder user);

        Task<SpotOrder?> GetByIdAsync(Guid id);

        Task<List<SpotOrder>> GetAllByUserIdAsync(Guid userId);

        Task<List<SpotOrder>> GetAllByCryptoIdAsync(Guid cryptoId);

        Task<List<SpotOrder>> GetAllOpenOrdersAsync();

        Task<List<SpotOrder>> GetOpenByUserIdAsync(Guid userId);

        Task<List<SpotOrder>> GetOpenByCryptoIdAsync(Guid cryptoId);

        void Remove(SpotOrder order);

        Task Update(SpotOrder order);
    }
}
