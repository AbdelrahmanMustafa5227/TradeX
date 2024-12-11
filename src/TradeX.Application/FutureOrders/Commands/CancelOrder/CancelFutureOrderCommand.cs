using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.FutureOrders.Commands.CancelOrder
{
    public record CancelFutureOrderCommand(Guid OrderId): ICommand
    {
    }
}
