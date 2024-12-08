using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders.Events;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Domain.FutureOrders
{
    public class FutureOrder : AggregateRoot , IOrder
    {
        private FutureOrder(Guid id, Guid userId, Guid cryptoId, FutureOrderType orderType, decimal amount, decimal entryPrice) : base(id)
        {
            UserId = userId;
            CryptoId = cryptoId;
            Type = orderType;
            Amount = amount;
            EntryPrice = entryPrice;
            IsActive = false;
            Total = 0;
            Fees = 0;
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


        public static FutureOrder Set(User user, Guid cryptoId , FutureOrderType orderType, decimal amount, decimal entryPrice)
        {
            return new FutureOrder(Guid.NewGuid(), user.Id, cryptoId, orderType, amount, entryPrice);
        }

        public Result CloseOrder(Crypto crypto, DateTime dateTime)
        {
            if (!IsActive)
                return Result.Failure(OrderErrors.OrderIsNotActive);

            if (CryptoId != crypto.Id)
                return Result.Failure(OrderErrors.CryptoInvalidOrNotSet);

            ExitPrice = crypto.Price;
            ClosedOnUtc = dateTime;
            IsActive = false;

            RaiseDomainEvent(new FutureOrderExecuted(this));
            return Result.Success();
        }

        public Result OpenOrder(DateTime dateTime)
        {
            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            OpenedOnUtc = dateTime;
            IsActive = true;

            return Result.Success();
        }

        public Result SetStopLoseTakeProfitPrice(Crypto crypto, decimal tp, decimal sl)
        {
            if (CryptoId != crypto.Id)
                return Result.Failure(OrderErrors.CryptoInvalidOrNotSet);

            if (sl > EntryPrice && Type == FutureOrderType.Long && !IsActive
                || sl < EntryPrice && Type == FutureOrderType.Short && !IsActive
                || sl > crypto.Price && Type == FutureOrderType.Long && IsActive
                || sl < crypto.Price && Type == FutureOrderType.Short && IsActive)
                return Result.Failure(OrderErrors.InvalidStopLossPrice);


            if (tp > EntryPrice && Type == FutureOrderType.Short && !IsActive
                || tp < EntryPrice && Type == FutureOrderType.Long && !IsActive
                || tp > crypto.Price && Type == FutureOrderType.Short && IsActive
                || tp < crypto.Price && Type == FutureOrderType.Long && IsActive)
                return Result.Failure(OrderErrors.InvalidTakeProfitPrice);

            StopLossPrice = sl;
            TakeProfitPrice = tp;
            return Result.Success();
        }

        public Result SetEntryPrice(decimal entryPrice)
        {
            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            if(TakeProfitPrice is null && StopLossPrice is null)
            {
                EntryPrice = entryPrice;
                return Result.Success();
            }

            if( (entryPrice <= StopLossPrice || entryPrice >= TakeProfitPrice) && Type == FutureOrderType.Long)
                return Result.Failure(OrderErrors.InvalidEntryPrice);

            if ((entryPrice >= StopLossPrice || entryPrice <= TakeProfitPrice) && Type == FutureOrderType.Short)
                return Result.Failure(OrderErrors.InvalidEntryPrice);

            EntryPrice = entryPrice;
            return Result.Success();
        }

        public Result ModifyOrder(FutureOrderType orderType, decimal amount)
        {
            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            Type = orderType;
            Amount = amount;
            return Result.Success();
        }

        public Result CancelOrder()
        {
            if (IsActive)
                return Result.Failure(OrderErrors.OrderAlreadyActive);

            RaiseDomainEvent(new FutureOrderCancelled(this));
            return Result.Success();
        }

        public void UpdatePricing(OrderDetails orderDetails)
        {
            var oldTotal = Total;
            Fees = orderDetails.Fees;
            Total = orderDetails.Total;
            RaiseDomainEvent(new FutureOrderPricingChanged(oldTotal, this));
        }

#pragma warning disable
        private FutureOrder() { }
#pragma warning enable
    }
}
