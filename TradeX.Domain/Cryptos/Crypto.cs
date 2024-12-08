using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Orders;

namespace TradeX.Domain.Cryptos
{
    public class Crypto : AggregateRoot
    {

        private Crypto(Guid id , string name , string symbol , decimal price , long totalSupply) : base(id)
        {
            Name = name;
            Symbol = symbol;
            Price = price;
            PriceLast24H = price;
            TotalSupply = totalSupply;
        }

        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public decimal Price { get; private set; }
        public decimal PriceLast24H { get; private set; }
        public decimal TradingVolume24H { get; private set; }
        public long TotalSupply { get; private set; }
        public decimal MarketCap => TotalSupply * Price;

        public static Crypto Create(string name, string symbol, decimal price, long totalSupply)
        {
            return new Crypto(Guid.NewGuid() , name , symbol , price , totalSupply);
        }


#pragma warning disable
        private Crypto() { }
#pragma warning enable
    }
}
