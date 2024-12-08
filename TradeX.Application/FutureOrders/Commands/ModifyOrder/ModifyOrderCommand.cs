using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;

namespace TradeX.Application.FutureOrders.Commands.ModifyOrder
{
    internal record ModifyOrderCommand (Guid OrderId, FutureOrderType OrderType , decimal Amount , decimal EntryPrice ) : ICommand
    {

    }
}
