using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.FutureOrders
{
    public static class FutureOrderErrors
    {
        public static readonly Error OrderAlreadyActive = new Error("Order", "Order is Open or has been executed");

        public static readonly Error OrderIsNotActive = new Error("Order", "Inactive orders cannot be closed");

        public static readonly Error OrderNotFound = new Error("Order", "order cannot be found");

        public static readonly Error InvalidExitPrice = new Error("Order", "Invalid exit Price");

        public static readonly Error InvalidEntryPrice = new Error("Order", "Invalid entry Price");

        public static readonly Error InvalidStopLossPrice = new Error("Order", "Invalid SLPrice");

        public static readonly Error InvalidTakeProfitPrice = new Error("Order", "Invalid TP Price");

        public static readonly Error CryptoInvalidOrNotSet = new Error("Order", "crypto Invalid or not set");

    }
}
