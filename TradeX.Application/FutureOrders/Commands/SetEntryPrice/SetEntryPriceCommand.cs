using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.FutureOrders.Commands.SetEntryPrice
{
    public record SetEntryPriceCommand(Guid OrderId , decimal EntryPrice) : ICommand
    {
    }
}
