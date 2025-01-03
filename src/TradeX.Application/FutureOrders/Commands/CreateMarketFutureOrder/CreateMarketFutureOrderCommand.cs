﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.FutureOrders;

namespace TradeX.Application.FutureOrders.Commands.CreateMarketFutureOrder
{
    public record CreateMarketFutureOrderCommand(Guid UserId, Guid CryptoId, FutureOrderType OrderType, decimal Amount) : ICommand
    {
    }
}
