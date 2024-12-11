using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Users;

namespace TradeX.Domain.FutureOrders
{
    public interface IFutureOrderRepository
    {
        void Add(FutureOrder user);

        Task<FutureOrder?> GetByIdAsync(Guid id);

        Task<List<FutureOrder>> GetAllByUserIdAsync(Guid userId);

        Task<List<FutureOrder>> GetAllByCryptoIdAsync(Guid cryptoId);

        Task<List<FutureOrder>> GetAllOpenOrdersAsync();

        Task<List<FutureOrder>> GetOpenByUserIdAsync(Guid userId);

        Task<List<FutureOrder>> GetOpenByCryptoIdAsync(Guid cryptoId);

        void Remove(FutureOrder order);

        Task Update(FutureOrder order);
    }
}
