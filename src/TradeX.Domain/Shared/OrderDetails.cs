using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Domain.Shared
{
    public record OrderDetails(decimal Volume, decimal Fees, decimal Total ,decimal Amount , bool IsSpotSellOrder , Guid? CryptoId)
    {

    }
}
