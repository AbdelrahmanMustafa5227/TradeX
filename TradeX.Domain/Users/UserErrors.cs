using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.Users
{
    public static class UserErrors
    {
        public static readonly Error PaymentMethodAlreadyConfirmed = new Error("User", "Payment Method Already Confirmed");

        public static readonly Error PaymentMethodNotConfirmed = new Error("User", "Payment Method is not Confirmed");

        public static readonly Error NoEnoughFunds = new Error("User", "No enough balance to complete this operation");

        public static readonly Error SubcriptionAlreadyActive = new Error("User", "User already Have an Active subsciption");

        public static readonly Error ExceededAlertLimit = new Error("User", "User has exceeded the maximum limit of alerts");

        public static readonly Error AlertNotFound = new Error("User", "Provided alert Id is not found");

        public static readonly Error AssetNotFound = new Error("User", "Provided asset Id is not found");
    }
}
