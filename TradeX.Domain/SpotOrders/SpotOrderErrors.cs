using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.SpotOrders
{
    public static class SpotOrderErrors
    {
        public static Error SpotOrderAlreadyExecuted = new Error("SpotOrder", "Spot Order Is Executed");

        public static Error SpotOrderNotFound = new Error("SpotOrder", "Spot Order Not Found");
    }
}
