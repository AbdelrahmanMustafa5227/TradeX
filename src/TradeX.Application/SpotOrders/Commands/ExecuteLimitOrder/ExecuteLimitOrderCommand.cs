using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.SpotOrders.Commands.ExecuteLimitOrder
{
    public record ExecuteLimitOrderCommand (Guid OrderId) : ICommand
    {
    }
}
