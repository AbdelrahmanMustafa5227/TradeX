using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.Users
{
    public record Balance
    {
        public decimal TotalBalance { get; init; }
        public decimal FreezedBalance { get; init; }
        public decimal AvailableBalance => TotalBalance - FreezedBalance;

        public Balance(decimal totalBalance = 0, decimal freezedBalance = 0)
        { 
            if(freezedBalance > totalBalance)
                throw new DomainException("Freezed balance Cannot be > Total balance");

            if (freezedBalance < 0)
                throw new DomainException("Freezed balance Cannot be < 0");

            if (totalBalance < 0)
                throw new DomainException("Total balance Cannot be < 0");

            TotalBalance = totalBalance;
            FreezedBalance = freezedBalance;
        }

        public static Balance operator +(Balance a, decimal b) 
        {
            return new Balance(a.TotalBalance + b, a.FreezedBalance);
        }

        public static Balance operator -(Balance a, decimal b)
        {
            return new Balance(a.TotalBalance - b, a.FreezedBalance);
        }
    }
}
