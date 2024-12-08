using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.FutureOrders.Commands.OpenOrder
{
    internal record OpenOrderCommand(Guid OrderId) : ICommand
    {
    }
}
