﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.Orders.Events
{
    public record OrderExecuted(Order order) : IDomainEvent
    {

    }
}
