using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.FutureOrders.Commands.OpenOrder
{
    public record OpenFutureOrderCommand(Guid OrderId) : ICommand
    {
    }
}
