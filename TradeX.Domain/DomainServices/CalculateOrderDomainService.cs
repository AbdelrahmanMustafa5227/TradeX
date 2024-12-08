using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Domain.DomainServices
{
    public class CalculateOrderDomainService
    {
        public OrderDetails CalculateOrderDetails(decimal entryPrice, decimal amount , Subscription subscription , DateTime now)
        {
            var volume = entryPrice * amount;

            var fees = volume * subscription.GetFeesPercentage(now);
            var total = volume + fees;

            return new OrderDetails(volume, fees, total);
        }

        public decimal GetOrderTotal(FutureOrder order, Crypto crypto, Subscription subscription, DateTime now)
        {
            var volume = crypto.Price * order.Amount;
            var fees = volume * subscription.GetFeesPercentage(now);
            var total = volume + fees;

            return total;
        }

        public Result<decimal> GetOrderFees(FutureOrder order, Crypto crypto, Subscription subscription, DateTime now)
        {
            var volume = crypto.Price * order.Amount;
            var fees = volume * subscription.GetFeesPercentage(now);

            return fees;
        }
    }
}
