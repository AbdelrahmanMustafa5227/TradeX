using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Domain.FutureOrders
{
    public record OrderDetails(decimal Volume , decimal Fees , decimal Total)
    {

    }
}
