using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Orders;

namespace TradeX.Application.FutureOrders.Commands.SetTPSLPrice
{
    internal record SetTPSLPriceCommand(Guid OrderId, decimal TPPrice , decimal SLPrice ) :ICommand
    {

    }
}
