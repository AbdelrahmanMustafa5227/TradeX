using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Orders;
using TradeX.Domain.Users;

namespace TradeX.Domain.DomainServices
{
    public class PerformanceMetricsDomainService
    {
        public UserPerformanceMetrics CalculateUserPerformance(List<Order> userOrders)
        {
            var closedOrders = userOrders.Where(x => x.ClosedOnUtc is not null).ToList();

            decimal totalProfit = 0;
            float wins = 0;
            decimal totalTradingVolume = 0m;

            foreach (var order in closedOrders)
            {
                var diff = (decimal)order.ExitPrice! - order.EntryPrice;
                
                if (diff > 0)
                    wins++;

                totalProfit += diff;
                totalTradingVolume += (order.EntryPrice + order.ExitPrice).Value;
            }

            double winLossRatio = wins / closedOrders.Count * 100;
            double totalProfitRatio = (double)totalProfit / closedOrders.Count * 100;
            return new UserPerformanceMetrics(winLossRatio, totalProfit, totalProfitRatio , totalTradingVolume);

        }

        public CryptoPerformanceMetrics CalculateCryptoPerformance(List<Order> CryptoOrders)
        {
            var transactionsInLast24H = CryptoOrders.Where(x => x.OpenedOnUtc?.Date == DateTime.UtcNow.Date 
                && (x.IsActive == true || x.ClosedOnUtc != null));


            var totalTradingVolume = 0m;

            foreach (var order in transactionsInLast24H)
            {
                if (order.ClosedOnUtc != null)
                    totalTradingVolume += order.ExitPrice!.Value;

                totalTradingVolume += order.EntryPrice;
            }

            return new CryptoPerformanceMetrics(totalTradingVolume);
        }
    }
}
