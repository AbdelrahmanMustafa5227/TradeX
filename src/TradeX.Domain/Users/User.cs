using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Shared;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Subscriptions.Events;
using TradeX.Domain.Users.Events;

namespace TradeX.Domain.Users
{
    public class User : AggregateRoot
    {
        private readonly List<Guid> _spotOrders = new();
        private readonly List<Guid> _futureOrders = new();
        private readonly List<Asset> _assets = new();


        private User(Guid id, string firstName, string lastName, string email, string password,
            DateTime createdOn, PaymentMethod paymentMethod) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            CreatedOnUtc = createdOn;
            balance = new Balance();
            PaymentMethod = paymentMethod;
            KYC_Confirmed = false;
            performanceMetrics = new UserPerformanceMetrics(0, 0, 0, 0);
        }

        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public DateTime CreatedOnUtc { get; private set; }
        public Balance balance { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public bool KYC_Confirmed { get; private set; }
        public Guid? SubscriptionId { get; private set; }
        public UserPerformanceMetrics performanceMetrics { get; private set; }

        public IReadOnlyList<Asset> Assets => _assets.ToList();
        public IReadOnlyList<Guid> SpotOrders => _spotOrders;
        public IReadOnlyList<Guid> FutureOrders => _futureOrders;

        public static User Create(string firstName, string lastName, string email, string password,
            DateTime createdOn, PaymentMethod paymentMethod)
        {
            return new User(Guid.NewGuid(), firstName, lastName, email, password, createdOn, paymentMethod);
        }

        public Result RequestKYCConfirmation()
        {
            if (KYC_Confirmed)
                return Result.Failure(UserErrors.KYCAlreadyConfirmed);

            RaiseDomainEvent(new KYCVerificationRequested(Id));
            return Result.Success();
        }

        public Result ConfirmKYC()
        {
            if (KYC_Confirmed)
                return Result.Failure(UserErrors.KYCAlreadyConfirmed);

            KYC_Confirmed = true;
            RaiseDomainEvent(new KYCRequestConfirmed(Id));
            return Result.Success();
        }

        public Result SetSubscription(Subscription subscription)
        {
            if (!KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);
            if (SubscriptionId is not null)
                return Result.Failure(UserErrors.SubcriptionAlreadyActive);
            if (balance.AvailableBalance < subscription.GetPrice())
                return Result.Failure(UserErrors.NoEnoughFunds);


            SubscriptionId = subscription.Id;
            balance -= subscription.GetPrice();
            RaiseDomainEvent(new SubscriptionSet(subscription));
            return Result.Success();
        }

        public Result Deposit(decimal amount)
        {
            if (!KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);

            balance += amount;
            return Result.Success();
        }

        public Result Withdraw(decimal amount)
        {
            if (!KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);
            if (balance.AvailableBalance < amount)
                return Result.Failure(UserErrors.NoEnoughFunds);

            balance -= amount;
            return Result.Success();
        }

        public Result Freeze(decimal amount)
        {
            if (!KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);

            balance = balance.Freeze(amount);
            return Result.Success();
        }

        public Result UnFreeze(decimal amount)
        {
            if (!KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);

            balance = balance.UnFreeze(amount);
            return Result.Success();
        }

        public Result Transfer(decimal amount, User receiver)
        {
            if (!KYC_Confirmed || !receiver.KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);
            if (balance.AvailableBalance < amount)
                return Result.Failure(UserErrors.NoEnoughFunds);

            balance -= amount;
            receiver.balance += amount;
            return Result.Success();
        }

        public void AddAsset(Guid cryptoId, decimal amount)
        {
            var oldAsset = _assets.FirstOrDefault(x => x.CryptoId == cryptoId);

            if (oldAsset is null)
                _assets.Add(Asset.Create(cryptoId,amount));
            else
                oldAsset.Add(amount);
        }

        public Result RemoveAsset(Guid cryptoId, decimal amount)
        {
            var asset = _assets.FirstOrDefault(x => x.CryptoId == cryptoId);

            if (asset is null)
                return Result.Failure(UserErrors.AssetNotFound);

            if (asset.Amount < amount)
                return Result.Failure(UserErrors.NoEnoughFunds);

            asset.Sub(amount);
            return Result.Success();
        }

        public Result AddOrder(IOrder order)
        {
            if (order is SpotOrder)
            {
                if (_spotOrders.Contains(order.Id))
                    return Result.Failure(UserErrors.OrderAlreadyExist);
                _spotOrders.Add(order.Id);
            }
            else
            {
                if (_futureOrders.Contains(order.Id))
                    return Result.Failure(UserErrors.OrderAlreadyExist);
                _futureOrders.Add(order.Id);
            }
            return Result.Success();
        }
        
        public Result RemoveOrder(IOrder order)
        {
            if (order is SpotOrder)
            {
                if (!_spotOrders.Contains(order.Id))
                    return Result.Failure(UserErrors.OrderNotFound);
                _spotOrders.Remove(order.Id);
            }
            else
            {
                if (!_futureOrders.Contains(order.Id))
                    return Result.Failure(UserErrors.OrderNotFound);
                _futureOrders.Remove(order.Id);
            }
            return Result.Success();
        }

        public bool CanAffordOrder(OrderDetails orderDetails)
        {
            if (orderDetails.IsSpotSellOrder)
            {
                var asset = Assets.FirstOrDefault(x => x.CryptoId == orderDetails.CryptoId);
                if (asset is null || asset.Amount < orderDetails.Amount)
                    return false;
            }
            else
            {
                if (balance.AvailableBalance < orderDetails.Total)
                    return false;
            }

            return true;
        }

#pragma warning disable
        private User() { }
#pragma warning enable
    }
}
