using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Orders;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Users;

namespace UnitTests.Domain.Orders
{
    public static class SpotOrderData
    {
        public static Result<SpotOrder> CreateMarket(Guid userId, Crypto crypto)
        {
            var res = SpotOrder.PlaceMarket(userId, crypto.Id, SpotOrderType.Buy, 1.5m, 91_000, 100,
                91_100, DateTime.UtcNow);
            return res;
        }

        public static Result<SpotOrder> CreateLimit(Guid userId, Crypto crypto)
        {
            var res = SpotOrder.PlaceLimit(userId, crypto.Id, SpotOrderType.Buy, 1.5m, 91_000, 100, 91_100);
            return res;
        }
    }
}
