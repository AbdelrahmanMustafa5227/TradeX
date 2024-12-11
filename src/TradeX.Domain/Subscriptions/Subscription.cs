using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Orders;
using TradeX.Domain.Subscriptions.Events;
using TradeX.Domain.Users;
using TradeX.Domain.Users.Events;

namespace TradeX.Domain.Subscriptions
{
    public class Subscription : AggregateRoot
    {
        private readonly List<Alert> _alerts = new();

        private Subscription(Guid id, Guid userId, SubscriptionTier tier, SubscriptionPlan plan, DateOnly startDate) : base(id)
        {
            UserId = userId;
            subscriptionTier = tier;
            subscriptionPlan = plan;
            StartDate = startDate;
            EndDate = GetEndDate(startDate);
            IsVaild = true;
        }
        
        public Guid UserId { get; private set; }
        public SubscriptionTier subscriptionTier { get; private set; }
        public SubscriptionPlan subscriptionPlan { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public bool IsVaild { get; private set; }
        public decimal ComulativeTradingVolume24H { get; private set; }

        public IReadOnlyList<Alert> Alerts => _alerts.ToList();

        public static Subscription Create(Guid userId, SubscriptionTier tier, SubscriptionPlan plan , DateOnly startDate)
        {
            return new Subscription(Guid.NewGuid(), userId, tier,plan, startDate);
        }

        public Result AddAlert(Alert alert ,DateTime Now)
        {
            if (_alerts.Count >= GetMaxALerts(Now))
                return Result.Failure(SubscriptionErrors.ExceededAlertLimit);

            _alerts.Add(alert);
            return Result.Success();
        }

        public Result RemoveAlert(Guid alertId)
        {
            var alert = _alerts.FirstOrDefault(x => x.Id == alertId);

            if (alert is null)
                return Result.Failure(SubscriptionErrors.AlertNotFound);

            _alerts.Remove(alert);
            return Result.Success();
        }

        public Result RenewSubscription(DateOnly utcNow)
        {
            if (IsVaild)
                return Result.Failure(UserErrors.SubcriptionAlreadyActive);

            StartDate = utcNow;
            EndDate = GetEndDate(utcNow);
            IsVaild = true;
            ComulativeTradingVolume24H = 0;

            RaiseDomainEvent(new SubscriptionRenewed(this));
            return Result.Success();
        }

        public void UpdateDailyComulativeVolume(decimal volume)
        {
            ComulativeTradingVolume24H += volume;
        }

        public bool CheckValidity(DateOnly today)
        {
            if(today >= EndDate)
                IsVaild = false;
            return IsVaild;
        }

        public decimal GetTradingVolumeLimit(DateTime now)
        {
            if (!IsVaild)
                return 5000;

            return subscriptionTier switch
            {
                SubscriptionTier.Basic => 5_000,
                SubscriptionTier.Bronze => 20_000,
                SubscriptionTier.Silver => 100_000,
                SubscriptionTier.Gold => 1_000_000,
                _ => throw new DomainException("Subscription Tier Is Not Found")
            };
        }

        public decimal GetFeesPercentage(DateTime now)
        {
            if (!IsVaild)
                return 0.05m;

            return subscriptionTier switch
            {
                SubscriptionTier.Basic => 0.05m,
                SubscriptionTier.Bronze => 0.03m,
                SubscriptionTier.Silver => 0.02m,
                SubscriptionTier.Gold => 0.01m,
                _ => throw new DomainException("Subscription Tier Is Not Found")
            };
        }

        public int GetMaxALerts(DateTime now)
        {
            if (!IsVaild)
                return 3;

            return subscriptionTier switch
            {
                SubscriptionTier.Basic => 3,
                SubscriptionTier.Bronze => 5,
                SubscriptionTier.Silver => 10,
                SubscriptionTier.Gold => int.MaxValue,
                _ => throw new DomainException("Subscription Tier Is Not Found")
            };
        }

        public decimal GetPrice()
        {
            if (subscriptionTier == SubscriptionTier.Basic)
                return 0m;

            decimal basePrice  = subscriptionTier switch
            {
                SubscriptionTier.Bronze => 10,
                SubscriptionTier.Silver => 20,
                SubscriptionTier.Gold => 30,
                _ => throw new DomainException("Invalid Subscription Tier")
            };

            (int multiplier, decimal discount) = subscriptionPlan switch
            {
                SubscriptionPlan.Monthly => (1,0),
                SubscriptionPlan.Quarter => (3,5),
                SubscriptionPlan.Half => (6,10),
                SubscriptionPlan.Yearly => (12,20),
                _ => throw new DomainException("Invalid Subscriptio Plan")
            };

            var totalPrice = basePrice * multiplier;

            return totalPrice - (discount * totalPrice / 100 );
        }

        private DateOnly GetEndDate(DateOnly startDate)
        {
            return subscriptionPlan switch
            {
                SubscriptionPlan.Quarter => startDate.AddMonths(3),
                SubscriptionPlan.Half => startDate.AddMonths(6),
                SubscriptionPlan.Yearly => startDate.AddYears(1),
                _ => startDate.AddMonths(1)
            };
        }


#pragma warning disable
        private Subscription() { }
#pragma warning enable
    }
}
