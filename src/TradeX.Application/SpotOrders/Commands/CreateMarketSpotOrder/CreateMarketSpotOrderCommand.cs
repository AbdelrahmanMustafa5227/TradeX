using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.SpotOrders;

namespace TradeX.Application.SpotOrders.Commands.CreateMarketSpotOrder
{
    public record CreateMarketSpotOrderCommand(Guid UserId, Guid CryptoId, SpotOrderType orderType, decimal Amount) : ICommand
    {
    }
}
