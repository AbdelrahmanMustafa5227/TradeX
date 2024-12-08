using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.SpotOrders.Commands.ChangeEntryPrice
{
    public record ChangeEntryPriceCommand(Guid OrderId , decimal EntryPrice) : ICommand
    {
    }
}
