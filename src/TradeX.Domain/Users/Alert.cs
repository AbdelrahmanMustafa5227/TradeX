using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.Users
{
    public class Alert : Entity
    {
        public decimal Price { get; private set; }
        public Guid CryptoId { get; private set; }
        public bool IsActive { get; private set; }

        private Alert(Guid Id , Guid cryptoId , decimal price) : base(Id)
        {
            CryptoId = cryptoId;
            Price = price;
            IsActive = true;
        }

        public static Alert Create(Guid cryptoId, decimal price)
        {
            return new Alert(Guid.NewGuid(), cryptoId, price);
        }

        private Alert() { }
    }
}
