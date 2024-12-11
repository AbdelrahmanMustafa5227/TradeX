using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.SpotOrders.Commands.CancelSpotOrder
{
    public record CancelSpotOrderCommand(Guid OrderId) : ICommand
    {
    }
}
