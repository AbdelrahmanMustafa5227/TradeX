using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace UnitTests.Domain.Subscrption
{
    public static class SubscriptionData
    {

        public static readonly SubscriptionTier tier = SubscriptionTier.Basic;
        public static readonly SubscriptionPlan plan = SubscriptionPlan.Monthly;
        public static readonly DateOnly startDate = DateOnly.FromDateTime(DateTime.UtcNow);

        public static Subscription Create(User user)
        {
            return Subscription.Create(user.Id, tier, plan , startDate);
        }

        public static Subscription CreateBronze(User user)
        {
            return Subscription.Create(user.Id, SubscriptionTier.Bronze, plan, startDate);
        }
    }
}
