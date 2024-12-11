using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.FutureOrders;

namespace TradeX.Domain.Orders.Events
{
    public record FutureOrderCancelled(FutureOrder Order) : IDomainEvent
    {
    }
}
