using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.FutureOrders.Events
{
    internal record FutureOrderPricingChanged(decimal oldPrice , FutureOrder order) : IDomainEvent
    {
    }
}
