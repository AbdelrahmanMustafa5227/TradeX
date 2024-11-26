using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Users;

namespace TradeX.Domain.Orders
{
    public interface IOrderRepository
    {
        void Add(Order user);

        Task<Order?> GetByIdAsync(Guid id);

        Task<List<Order>> GetAllByUserIdAsync(Guid userId);

        Task<List<Order>> GetAllByCryptoIdAsync(Guid cryptoId);

        Task<List<Order>> GetAllOpenOrdersAsync();

        Task<List<Order>> GetOpenByUserIdAsync(Guid userId);

        Task<List<Order>> GetOpenByCryptoIdAsync(Guid cryptoId);

        Task Remove(Order id);
    }
}
