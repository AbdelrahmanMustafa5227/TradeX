using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.Users
{
    public class Asset : Entity
    {
        public Guid CryptoId { get; private set; }
        public decimal Amount { get; private set; }
        
        
        private Asset(Guid Id ,Guid cryptoId , decimal amount) :base(Id)
        {
            if (amount < 0)
                throw new DomainException("Asset Amount Can't Be < 0");

            CryptoId = cryptoId;
            Amount = amount;
        }

        public static Asset Create(Guid CryptoId ,decimal Amount)
        {
            return new Asset(Guid.NewGuid(), CryptoId, Amount);
        }

        public void Add(decimal amount)
        {
            Amount += amount;
        }

        public void Sub(decimal amount)
        {
            Amount -= amount;
        }

        private Asset()
        {

        }
    }
}
