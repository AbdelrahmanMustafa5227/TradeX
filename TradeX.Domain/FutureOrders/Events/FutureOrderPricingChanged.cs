using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.FutureOrders.Events
{
    public record FutureOrderPricingChanged(decimal OldTotal , FutureOrder Order) : IDomainEvent
    {
    }
}
