using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders.Events;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.Shared;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Domain.FutureOrders
{
    public class FutureOrder : AggregateRoot, IOrder
    {
        private FutureOrder(Guid id, Guid userId, Guid cryptoId, FutureOrderType orderType, decimal amount,
            decimal entryPrice, bool isActive, decimal fees, decimal total, DateTime? openedOn = null) : base(id)
        {
            UserId = userId;
            CryptoId = cryptoId;
            Type = orderType;
            Amount = amount;
            EntryPrice = entryPrice;
            IsActive = isActive;
            Total = total;
            Fees = fees;
            OpenedOnUtc = openedOn;
        }

        public Guid UserId { get; private set; }
        public Guid CryptoId { get; private set; }

        public FutureOrderType Type { get; private set; }
        public decimal Amount { get; private set; }

        public decimal EntryPrice { get; private set; }
        public decimal? ExitPrice { get; private set; }
        public decimal? StopLossPrice { get; private set; }
        public decimal? TakeProfitPrice { get; private set; }

        public bool IsActive { get; private set; }

        public decimal Fees { get; private set; }
        public decimal Total { get; private set; }

        public DateTime? OpenedOnUtc { get; private set; }
        public DateTime? ClosedOnUtc { get; private set; }


        public static FutureOrder PlaceLimit(Guid userId, Guid cryptoId, FutureOrderType orderType, decimal amount,
            decimal entryPrice, decimal fees, decimal total)
        {
            var order = new FutureOrder(Guid.NewGuid(), userId, cryptoId, orderType, amount, entryPrice, false, fees, total);
            order.RaiseDomainEvent(new LimitFutureOrderPlaced(order));
            return order;
        }

        public static FutureOrder PlaceMarket(Guid userId, Guid cryptoId, FutureOrderType orderType, decimal amount,
            decimal entryPrice, decimal fees, decimal total, DateTime openedOn)
        {
            var order = new FutureOrder(Guid.NewGuid(), userId, cryptoId, orderType, amount, entryPrice, true, fees, total, openedOn);
            order.RaiseDomainEvent(new MarketFutureOrderPlaced(order));
            return order;
        }

        public Result CloseOrder(decimal exitPrice , DateTime dateTime)
        {
            if (!IsActive)
                return Result.Failure(FutureOrderErrors.OrderIsNotActive);


            ExitPrice = exitPrice;
            ClosedOnUtc = dateTime;
            IsActive = false;

            RaiseDomainEvent(new FutureOrderClosed(this));
            return Result.Success();
        }

        public Result OpenOrder(DateTime dateTime)
        {
            if (IsActive)
                return Result.Failure(FutureOrderErrors.OrderAlreadyActive);

            OpenedOnUtc = dateTime;
            IsActive = true;

            RaiseDomainEvent(new FutureOrderOpened(this));
            return Result.Success();
        }

        public Result SetStopLoseTakeProfitPrice(Crypto crypto, decimal tp, decimal sl)
        {
            if (CryptoId != crypto.Id)
                return Result.Failure(FutureOrderErrors.CryptoInvalidOrNotSet);

            if (sl >= EntryPrice && Type == FutureOrderType.Long && !IsActive
                || sl <= EntryPrice && Type == FutureOrderType.Short && !IsActive
                || sl >= crypto.Price && Type == FutureOrderType.Long && IsActive
                || sl <= crypto.Price && Type == FutureOrderType.Short && IsActive)
                return Result.Failure(FutureOrderErrors.InvalidStopLossPrice);


            if (tp >= EntryPrice && Type == FutureOrderType.Short && !IsActive
                || tp <= EntryPrice && Type == FutureOrderType.Long && !IsActive
                || tp >= crypto.Price && Type == FutureOrderType.Short && IsActive
                || tp <= crypto.Price && Type == FutureOrderType.Long && IsActive)
                return Result.Failure(FutureOrderErrors.InvalidTakeProfitPrice);

            StopLossPrice = sl;
            TakeProfitPrice = tp;
            return Result.Success();
        }

        public Result SetEntryPrice(decimal entryPrice, OrderDetails orderDetails)
        {
            if (IsActive || ClosedOnUtc is not null)
                return Result.Failure(FutureOrderErrors.OrderAlreadyActive);

            if (TakeProfitPrice is not null && StopLossPrice is not null)
            {
                if ((entryPrice <= StopLossPrice || entryPrice >= TakeProfitPrice) && Type == FutureOrderType.Long)
                    return Result.Failure(FutureOrderErrors.InvalidEntryPrice);

                if ((entryPrice >= StopLossPrice || entryPrice <= TakeProfitPrice) && Type == FutureOrderType.Short)
                    return Result.Failure(FutureOrderErrors.InvalidEntryPrice);
            }

            var oldTotal = Total;

            EntryPrice = entryPrice;
            Total = orderDetails.Total;
            Fees = orderDetails.Fees;
            RaiseDomainEvent(new FutureOrderPricingChanged(oldTotal, this));
            return Result.Success();
        }

        public Result ModifyOrder(decimal amount, OrderDetails orderDetails)
        {
            if (IsActive || ClosedOnUtc is not null)
                return Result.Failure(FutureOrderErrors.OrderAlreadyActive);

            var oldTotal = Total;
            Amount = amount;
            Total = orderDetails.Total;
            Fees = orderDetails.Fees;
            RaiseDomainEvent(new FutureOrderPricingChanged(oldTotal, this));
            return Result.Success();
        }

        public Result CancelOrder()
        {
            if (IsActive || ClosedOnUtc is not null)
                return Result.Failure(FutureOrderErrors.OrderAlreadyActive);

            RaiseDomainEvent(new FutureOrderCancelled(this));
            return Result.Success();
        }


#pragma warning disable
        private FutureOrder() { }
#pragma warning enable
    }
}
