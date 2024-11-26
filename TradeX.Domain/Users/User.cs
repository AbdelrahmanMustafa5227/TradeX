using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users.Events;

namespace TradeX.Domain.Users
{
    public class User : AggregateRoot
    {
        private readonly List<Guid> _orders = new();
        private readonly List<Asset> _assets = new();
        private readonly List<Alert> _alerts = new();

        private User(Guid id, string firstName , string lastName , string email , string password,
            DateTime createdOn , PaymentMethod paymentMethod ) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            CreatedOnUtc = createdOn;
            balance = new Balance();
            PaymentMethod = paymentMethod;
            KYC_Confirmed = false;
            performanceMetrics = new UserPerformanceMetrics(0, 0, 0 ,0);
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
        public IReadOnlyList<Guid> Orders => _orders.ToList();
        public IReadOnlyList<Alert> Alerts => _alerts.ToList();

        public static User Create(string firstName, string lastName, string email, string password,
            DateTime createdOn, PaymentMethod paymentMethod)
        {
            return new User(Guid.NewGuid(), firstName, lastName, email, password, createdOn, paymentMethod);
        }

        public Result RequestPaymentMethodConfirmation()
        {
            if(KYC_Confirmed)
                return Result.Failure(UserErrors.PaymentMethodAlreadyConfirmed);

            RaiseDomainEvent(new PaymentMethodConfirmationRequestedDomainEvent(Id));
            return Result.Success();
        }

        public Result ConfirmPaymentMethod()
        {
            if (KYC_Confirmed)
                return Result.Failure(UserErrors.PaymentMethodAlreadyConfirmed);

            RaiseDomainEvent(new PaymentMethodConfirmedDomainEvent(Id));
            return Result.Success();
        }

        public Result SetSubscription(Subscription subscription)
        {
            if (!KYC_Confirmed)
                return Result.Failure(UserErrors.PaymentMethodNotConfirmed);
            if (SubscriptionId is not null)
                return Result.Failure(UserErrors.SubcriptionAlreadyActive);
            if (balance.AvailableBalance < subscription.Price)
                return Result.Failure(UserErrors.NoEnoughFunds);


            SubscriptionId = subscription.Id;
            balance -= subscription.Price;
            RaiseDomainEvent(new SubscriptionBoughtDomainEvent(Id,subscription));
            return Result.Success();
        }

        public Result Deposit(decimal amount)
        {
            if(!KYC_Confirmed)
                return Result.Failure(UserErrors.PaymentMethodNotConfirmed);

            balance += amount;
            return Result.Success();
        }

        public Result Withdraw(decimal amount)
        {
            if (!KYC_Confirmed)
                return Result.Failure(UserErrors.PaymentMethodNotConfirmed);
            if(balance.AvailableBalance < amount)
                return Result.Failure(UserErrors.NoEnoughFunds);

            balance -= amount;
            return Result.Success();
        }

        public Result Transfer(decimal amount , User user)
        {
            if (!KYC_Confirmed || !user.KYC_Confirmed)
                return Result.Failure(UserErrors.PaymentMethodNotConfirmed);
            if (balance.AvailableBalance < amount)
                return Result.Failure(UserErrors.NoEnoughFunds);

            balance -= amount;
            user.balance += amount;
            RaiseDomainEvent(new MoneyTransferredDomainEvent(Id, user.Id));
            return Result.Success();
        }

        public Result AddAlert(Alert alert , Subscription subscription)
        {
            if (_alerts.Count > subscription.MaxAlerts)
                return Result.Failure(UserErrors.ExceededAlertLimit);

            _alerts.Add(alert);
            return Result.Success();
        }

        public Result RemoveAlert(Alert alert)
        {
            if (_alerts.Contains(alert))
                return Result.Failure(UserErrors.AlertNotFound);

            _alerts.Remove(alert);
            return Result.Success();
        }

        public Result AddAsset(Guid assetId , decimal amount)
        {
            var asset = _assets.FirstOrDefault(x=> x.Id == assetId);
            if (asset is null)
            {
               _assets.Add(Asset.Create(assetId, amount));
            }
            else
            {
                asset.Add(amount);
            }
            return Result.Success();
        }

        public Result RemoveAsset(Guid assetId, decimal amount)
        {
            var asset = _assets.FirstOrDefault(x => x.Id == assetId);
            if (asset is null)
                return Result.Failure(UserErrors.AssetNotFound);

            if(asset.Amount < amount)
                return Result.Failure(UserErrors.NoEnoughFunds);

            asset.Sub(amount);
            return Result.Success();
        }



#pragma warning disable
        private User() { }
#pragma warning enable
    }
}
