using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.Users;

namespace TradeX.Domain.Orders
{
    public class Order : AggregateRoot
    {
        private Order(Guid id, Guid userId, OrderType orderType, decimal amount,
            decimal entryPrice, decimal? stopLoss = null, decimal? takeProfit = null) : base(id)
        {
            UserId = userId;
            Type = orderType;
            Amount = amount;
            EntryPrice = entryPrice;
            StopLossPrice = stopLoss;
            TakeProfitPrice = takeProfit;
            IsActive = false;
        }

        public Guid UserId { get; private set; }
        public OrderType Type { get; private set; }
        public decimal Amount { get; private set; }

        public decimal EntryPrice { get; private set; }
        public bool IsActive { get; private set; }

        public decimal? ExitPrice { get; private set; }
        public decimal? StopLossPrice { get; private set; }
        public decimal? TakeProfitPrice { get; private set; }
        
        public Guid? CryptoId { get; private set; }
        public DateTime? OpenedOnUtc { get; private set; }
        public DateTime? ClosedOnUtc { get; private set; }


        public static Order Set(Guid userId, OrderType orderType, decimal amount, decimal entryPrice, decimal? stopLoss = null, decimal? takeProfit = null)
        {
            return new Order(Guid.NewGuid(), userId, orderType, amount, entryPrice, stopLoss, takeProfit);
        }

        public Result PlaceMarketOrder(Crypto crypto, DateTime dateTime)
        {
            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            CryptoId = crypto.Id;
            ExitPrice = EntryPrice = crypto.Price;
            ClosedOnUtc = OpenedOnUtc = dateTime;

            RaiseDomainEvent(new OrderExecuted(this));
            return Result.Success();
        }

        public Result CloseOrder(Crypto crypto, DateTime dateTime)
        {
            if (!IsActive)
                return Result.Failure(OrderErrors.OrderIsNotActive);

            if (CryptoId is null || CryptoId != crypto.Id)
                return Result.Failure(OrderErrors.CryptoInvalidOrNotSet);

            ExitPrice = crypto.Price;
            ClosedOnUtc = dateTime;
            IsActive = false;

            RaiseDomainEvent(new OrderExecuted(this));
            return Result.Success();
        }

        public Result OpenOrder(Crypto crypto, DateTime dateTime)
        {
            if (CryptoId is null || CryptoId != crypto.Id)
                return Result.Failure(OrderErrors.CryptoInvalidOrNotSet);

            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            EntryPrice = crypto.Price;
            OpenedOnUtc = dateTime;
            IsActive = true;

            return Result.Success();
        }

        public Result SetStopLoseTakeProfitPrice(Crypto crypto, decimal sl, decimal tp)
        {
            if (CryptoId is null || CryptoId != crypto.Id)
                return Result.Failure(OrderErrors.CryptoInvalidOrNotSet);

            if (sl > EntryPrice && Type == OrderType.Long && !IsActive
                || sl < EntryPrice && Type == OrderType.Short && !IsActive
                || sl > crypto.Price && Type == OrderType.Long && IsActive
                || sl < crypto.Price && Type == OrderType.Short && IsActive)
                return Result.Failure(OrderErrors.InvalidStopLossPrice);


            if (tp > EntryPrice && Type == OrderType.Short && !IsActive
                || tp < EntryPrice && Type == OrderType.Long && !IsActive
                || tp > crypto.Price && Type == OrderType.Short && IsActive
                || tp < crypto.Price && Type == OrderType.Long && IsActive)
                return Result.Failure(OrderErrors.InvalidTakeProfitPrice);

            StopLossPrice = sl;
            TakeProfitPrice = tp;
            return Result.Success();
        }

        public Result SetEntryPrice(Crypto crypto, decimal price)
        {
            if (CryptoId is null || CryptoId != crypto.Id)
                return Result.Failure(OrderErrors.CryptoInvalidOrNotSet);

            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            EntryPrice = price;
            return Result.Success();
        }

        public Result ModifyOrder(Crypto crypto, OrderType orderType, decimal amount ,decimal entryPrice)
        {
            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            CryptoId = crypto.Id;
            Type = orderType;
            Amount = amount;
            EntryPrice = entryPrice;
            return Result.Success();
        }

        public Result CancelOrder()
        {
            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            RaiseDomainEvent(new OrderCancelled(this));
            return Result.Success();
        }

#pragma warning disable
        private Order() { }
#pragma warning enable
    }
}
