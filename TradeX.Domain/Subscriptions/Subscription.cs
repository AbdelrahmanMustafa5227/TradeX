using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Domain.Subscriptions
{
    public class Subscription : AggregateRoot
    {

        private Subscription(Guid id, Guid userId, SubscriptionTier tier, DateOnly startDate) : base(id)
        {
            MaxAlerts = GetMaxALerts();
            FeesPerOrderPercentage = GetFeesPercentage();
            DailyTradingVolumeLimit = GetTradingVolumeLimit();
            UserId = userId;
            subscriptionTier = tier;
            StartDate = startDate;
            EndDate = GetEndDate(startDate);
            Price = GetPrice();
        }

        public int MaxAlerts { get; private set; }
        public decimal FeesPerOrderPercentage { get; private set; }
        public double DailyTradingVolumeLimit { get; private set; }
        public Guid UserId { get; private set; }
        public SubscriptionTier subscriptionTier { get; private set; }
        public SubscriptionPlan subscriptionPlan { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public decimal Price { get; private set; }


        public static Subscription Create(Guid userId, SubscriptionTier tier, DateOnly startDate)
        {
            return new Subscription(Guid.NewGuid(), userId, tier, startDate);
        }


        private double GetTradingVolumeLimit()
        {
            return subscriptionTier switch
            {
                SubscriptionTier.Bronze => 20_000,
                SubscriptionTier.Silver => 100_000,
                SubscriptionTier.Gold => 1_000_000,
                _ => 5000
            };
        }

        private decimal GetFeesPercentage()
        {
            return subscriptionTier switch
            {
                SubscriptionTier.Bronze => 0.3m,
                SubscriptionTier.Silver => 0.2m,
                SubscriptionTier.Gold => 0.1m,
                _ => 0.5m
            };
        }

        private int GetMaxALerts()
        {
            return subscriptionTier switch
            {
                SubscriptionTier.Bronze => 5,
                SubscriptionTier.Silver => 10,
                SubscriptionTier.Gold => int.MaxValue,
                _ => 3
            };
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

        private decimal GetPrice()
        {
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


#pragma warning disable
        private Subscription() { }
#pragma warning enable
    }
}
