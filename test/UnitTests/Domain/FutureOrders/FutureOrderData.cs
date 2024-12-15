using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.SpotOrders;

namespace UnitTests.Domain.FutureOrders
{
    public class FutureOrderData
    {
        public static Result<FutureOrder> CreateMarket(Guid userId, Crypto crypto)
        {
            var res = FutureOrder.PlaceMarket(userId, crypto.Id, FutureOrderType.Long, 1.5m, 91_000, 100,
                91_100, DateTime.UtcNow);
            return res;
        }

        public static Result<FutureOrder> CreateLimit(Guid userId, Crypto crypto)
        {
            var res = FutureOrder.PlaceLimit(userId, crypto.Id, FutureOrderType.Long, 1.5m, 91_000, 100, 91_100);
            return res;
        }

        public static Result<FutureOrder> CreateLimitShort(Guid userId, Crypto crypto)
        {
            var res = FutureOrder.PlaceLimit(userId, crypto.Id, FutureOrderType.Short, 1.5m, 91_000, 100, 91_100);
            return res;
        }
    }
}
