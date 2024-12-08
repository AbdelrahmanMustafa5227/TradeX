using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Domain.Abstractions
{
    public interface IOrder
    {
        Guid Id { get; }
        decimal Amount { get; }
        decimal EntryPrice { get; }
    }
}
