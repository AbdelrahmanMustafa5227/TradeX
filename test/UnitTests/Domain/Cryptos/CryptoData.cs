using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Cryptos;

namespace UnitTests.Domain.Cryptos
{
    public class CryptoData
    {
        public static Crypto Create()
        {
            return Crypto.Create("BitCoin", "BTC", 90_000, 1_000_000);
        }
    }
}
