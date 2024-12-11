using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.SpotOrders;

namespace TradeX.Application.SpotOrders.Commands.ModifySpotOrder
{
    public record ModifySpotOrderCommand(Guid OrderId, SpotOrderType orderType, decimal Amount) : ICommand
    {
    }
}
