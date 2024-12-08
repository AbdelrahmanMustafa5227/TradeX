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
        public decimal TotalBalance { get; private set; }
        public decimal FreezedBalance { get; private set; }
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

        public Balance Freeze(decimal amount)
        {
            if(amount > AvailableBalance)
                throw new DomainException("Total balance Cannot be < amount to be freezed");
            return new Balance(TotalBalance, FreezedBalance + amount);
        }

        public Balance UnFreeze(decimal amount)
        {
            if (amount > FreezedBalance)
                throw new DomainException("freezed balance Cannot be < amount to be unfreezed");
            return new Balance(TotalBalance, FreezedBalance - amount);
        }
    }
}
