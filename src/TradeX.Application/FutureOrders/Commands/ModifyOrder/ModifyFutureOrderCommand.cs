using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;

namespace TradeX.Application.FutureOrders.Commands.ModifyOrder
{
    public record ModifyFutureOrderCommand (Guid OrderId, decimal Amount) : ICommand
    {

    }
}
