using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Orders;
using TradeX.Domain.Users;

namespace UnitTests.Domain.Orders
{
    public static class OrderData
    {
        
        public static Result<FutureOrder> CreateLong(User user)
        {
            var res = FutureOrder.Set(user, FutureOrderType.Long, 1.5m, 91_000);
            return res;
        }

        public static Result<FutureOrder> CreateShort(User user)
        {
            var res = FutureOrder.Set(user, OrderType.Short, 1.5m, 91_000);
            return res;
        }
    }
}
