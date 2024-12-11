using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Orders;

namespace TradeX.Application.FutureOrders.Commands.CreateOrder
{
    public record CreateLimitFutureOrderCommand(Guid UserId , Guid CryptoId , FutureOrderType OrderType , decimal EntryPrice , decimal Amount) : ICommand
    {

    }
}
