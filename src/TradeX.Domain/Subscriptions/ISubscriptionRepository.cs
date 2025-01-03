using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Shared;

namespace TradeX.Domain.Subscriptions
{
    public interface ISubscriptionRepository
    {
        void Add(Subscription subscription);

        Task<Subscription?> GetByIdAsync(Guid id);

        Task<Subscription?> GetByUserIdAsync(Guid userId);

        Task<PaginatedList<Subscription>> GetAllAsync(int page = 1, int pageSize = 0);

        void ZeriongAll();

    }
}
