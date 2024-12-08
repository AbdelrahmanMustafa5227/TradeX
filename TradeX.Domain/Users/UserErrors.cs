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
        public static readonly Error UserNotFound = new Error("User", "User Not Found");

        public static readonly Error EmailUsed = new Error("User", "Email is already registered");

        public static readonly Error KYCAlreadyConfirmed = new Error("User", "KYC Already Verified");

        public static readonly Error KYCNotConfirmed = new Error("User", "KYC is not Verified");

        public static readonly Error NoEnoughFunds = new Error("User", "No enough balance to complete this operation");

        public static readonly Error SubcriptionAlreadyActive = new Error("User", "User already Have an Active subsciption");

        public static readonly Error InvalidSubcription = new Error("User", "User already Have an Active subsciption");

        public static readonly Error AssetNotFound = new Error("User", "Provided asset Id is not found");

        public static readonly Error OrderAlreadyExist = new Error("User", "User already Have this order");

        public static readonly Error OrderNotFound = new Error("User", "User doesn't Have this order");
    }
}
