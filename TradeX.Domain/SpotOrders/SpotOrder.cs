using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.SpotOrders.Events;
using TradeX.Domain.Users;

namespace TradeX.Domain.SpotOrders
{
    public class SpotOrder : AggregateRoot, IOrder
    {

        private SpotOrder(Guid id, Guid userId, Guid cryptoId, SpotOrderType orderType, decimal amount, decimal entryPrice) : base(id)
        {
            UserId = userId;
            CryptoId = cryptoId;
            OrderType = orderType;
            Amount = amount;
            EntryPrice = entryPrice;
            Total = 0;
            Fees = 0;
            RaiseDomainEvent(new SpotOrderCreated(this));
        }

        public Guid UserId { get; private set; }
        public Guid CryptoId { get; private set; }
        public SpotOrderType OrderType { get; private set; }
        public decimal Amount { get; private set; }
        public decimal EntryPrice { get; private set; }

        public decimal Fees { get; private set; }
        public decimal Total { get; private set; }
        public DateTime? ExecutedOnUtc { get; private set; }


        public static SpotOrder Create(Guid userId, Guid cryptoId, SpotOrderType orderType, decimal amount, decimal entryPrice)
        {
            return new SpotOrder(Guid.NewGuid(), userId, cryptoId, orderType, amount, entryPrice);
        }

        public void ExecuteMarketOrder(OrderDetails orderDetails, DateTime dateTime)
        {
            Fees = orderDetails.Fees;
            Total = orderDetails.Total;
            ExecutedOnUtc = dateTime;
            RaiseDomainEvent(new MarketSpotOrderExecuted(this));
        }

        public void ExecuteLimitOrder(DateTime dateTime)
        {
            ExecutedOnUtc = dateTime;
            RaiseDomainEvent(new LimitSpotOrderExecuted(this));
        }


        public Result SetEntryPrice(decimal entryPrice)
        {
            if (ExecutedOnUtc is not null)
                return Result.Failure(SpotOrderErrors.SpotOrderAlreadyExecuted);

            EntryPrice = entryPrice;
            return Result.Success();
        }

        public Result ModifyOrder(SpotOrderType orderType, decimal amount)
        {
            if (ExecutedOnUtc is not null)
                return Result.Failure(SpotOrderErrors.SpotOrderAlreadyExecuted);

            OrderType = orderType;
            Amount = amount;
            return Result.Success();
        }

        public Result CancelOrder()
        {
            if (ExecutedOnUtc is not null)
                return Result.Failure(SpotOrderErrors.SpotOrderAlreadyExecuted);

            RaiseDomainEvent(new SpotOrderCancelled(this));
            return Result.Success();
        }

        public void UpdatePricing(decimal oldAmount, OrderDetails orderDetails)
        {
            var oldTotal = Total;
            Fees = orderDetails.Fees;
            Total = orderDetails.Total;
            RaiseDomainEvent(new SpotOrderPricingChanged(oldTotal, oldAmount, this));
        }

        private SpotOrder() { }
    }
}
