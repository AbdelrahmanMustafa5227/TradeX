using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.SpotOrders;

namespace TradeX.Application.SpotOrders.Commands.CreateLimitSpotOrder
{
    public record CreateLimitSpotOrderCommand(Guid UserId, Guid CryptoId, SpotOrderType orderType, decimal Amount , decimal EntryPrice) : ICommand
    {
    }
}
