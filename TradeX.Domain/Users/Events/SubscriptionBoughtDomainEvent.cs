using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions;

namespace TradeX.Domain.Users.Events
{
    public record SubscriptionBoughtDomainEvent(Guid userId , Subscription subscription) : IDomainEvent
    {

    }
}
