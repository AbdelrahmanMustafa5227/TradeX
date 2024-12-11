using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.Shared;
using TradeX.Domain.SpotOrders.Events;
using TradeX.Domain.Users;

namespace TradeX.Domain.SpotOrders
{
    public class SpotOrder : AggregateRoot, IOrder
    {

        private SpotOrder(Guid id, Guid userId, Guid cryptoId, SpotOrderType orderType, decimal amount,
            decimal entryPrice, decimal fees = 0, decimal total = 0, DateTime? executedOn = null) : base(id)
        {
            UserId = userId;
            CryptoId = cryptoId;
            OrderType = orderType;
            Amount = amount;
            EntryPrice = entryPrice;
            Total = total;
            Fees = fees;
            ExecutedOnUtc = executedOn;
        }

        public Guid UserId { get; private set; }
        public Guid CryptoId { get; private set; }
        public SpotOrderType OrderType { get; private set; }
        public decimal Amount { get; private set; }
        public decimal EntryPrice { get; private set; }

        public decimal Fees { get; private set; }
        public decimal Total { get; private set; }
        public DateTime? ExecutedOnUtc { get; private set; }


        public static SpotOrder PlaceLimit(Guid userId, Guid cryptoId, SpotOrderType orderType, decimal amount, decimal entryPrice,
            decimal fees, decimal total)
        {
            var order = new SpotOrder(Guid.NewGuid(), userId, cryptoId, orderType, amount, entryPrice, fees, total);
            order.RaiseDomainEvent(new LimitSpotOrderCreated(order));
            return order;
        }

        public static SpotOrder PlaceMarket(Guid userId, Guid cryptoId, SpotOrderType orderType, decimal amount,
            decimal entryPrice, decimal fees, decimal total, DateTime executedOn)
        {
            var order = new SpotOrder(Guid.NewGuid(), userId, cryptoId, orderType, amount, entryPrice, fees, total, executedOn);
            order.RaiseDomainEvent(new MarketSpotOrderExecuted(order));
            return order;
        }


        public void ExecuteLimitOrder(DateTime dateTime)
        {
            ExecutedOnUtc = dateTime;
            RaiseDomainEvent(new LimitSpotOrderExecuted(this));
        }


        public Result SetEntryPrice(decimal entryPrice , OrderDetails orderDetails)
        {
            if (ExecutedOnUtc is not null)
                return Result.Failure(SpotOrderErrors.SpotOrderAlreadyExecuted);

            var oldTotal = Total;

            EntryPrice = entryPrice;
            Fees = orderDetails.Fees;
            Total = orderDetails.Total;
            RaiseDomainEvent(new SpotOrderPricingChanged(oldTotal, Amount, this));
            return Result.Success();
        }

        public Result ModifyOrder(decimal amount, OrderDetails orderDetails)
        {
            if (ExecutedOnUtc is not null)
                return Result.Failure(SpotOrderErrors.SpotOrderAlreadyExecuted);

            var oldTotal = Total;
            var oldAmount = Amount;

            Amount = amount;
            Fees = orderDetails.Fees;
            Total = orderDetails.Total;
            RaiseDomainEvent(new SpotOrderPricingChanged(oldTotal, oldAmount, this));
            return Result.Success();
        }

        public Result CancelOrder()
        {
            if (ExecutedOnUtc is not null)
                return Result.Failure(SpotOrderErrors.SpotOrderAlreadyExecuted);

            RaiseDomainEvent(new SpotOrderCancelled(this));
            return Result.Success();
        }

        private SpotOrder() { }
    }
}
