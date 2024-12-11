using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Shared;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Domain.DomainServices
{
    public class CalculateOrderDomainService
    {
        public OrderDetails CalculateOrderDetails(decimal entryPrice, decimal amount , Subscription subscription , DateTime now ,
            bool isSpotSellOrder = false , Guid? CryptoId = null)
        {
            var volume = entryPrice * amount;
            var fees = volume * subscription.GetFeesPercentage(now);
            var total = volume + fees;

            return new OrderDetails(volume, fees, total , amount , isSpotSellOrder, CryptoId);
        }

        //public OrderDetails CalculateOrderDetails(IOrder order, Subscription subscription, DateTime now)
        //{
        //    var volume = order.EntryPrice * order.Amount;
        //    var fees = volume * subscription.GetFeesPercentage(now);
        //    var total = volume + fees;
            
        //    return new OrderDetails(volume, fees, total , order.Amount);
        //}
    }
}
