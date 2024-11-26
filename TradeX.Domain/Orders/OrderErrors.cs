using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.Orders
{
    public static class OrderErrors
    {
        public static readonly Error OrderAlreadyActive = new Error("Order", "Open Order");

        public static readonly Error OrderIsNotActive = new Error("Order", "Inactive orders cannot be closed");

        public static readonly Error InvalidExitPrice = new Error("Order", "Invalid Short Order Price");

        public static readonly Error InvalidStopLossPrice = new Error("Order", "Invalid Short Order Price");

        public static readonly Error InvalidTakeProfitPrice = new Error("Order", "Invalid Short Order Price");

        public static readonly Error CryptoInvalidOrNotSet = new Error("Order", "Invalid Short Order Price");

    }
}
