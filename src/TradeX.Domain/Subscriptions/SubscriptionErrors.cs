using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.Subscriptions
{
    public static class SubscriptionErrors
    {
        public static readonly Error ExceededAlertLimit = new Error("User", "User has exceeded the maximum limit of alerts");

        public static readonly Error ExceededDailyTradingVolumeLimit = new Error("User", "User has exceeded the maximum limit of trading volume");

        public static readonly Error AlertNotFound = new Error("User", "Provided alert Id is not found");

        public static readonly Error SubscriptionNotFound = new Error("User", "Provided sub Id is not found");

    }
}
