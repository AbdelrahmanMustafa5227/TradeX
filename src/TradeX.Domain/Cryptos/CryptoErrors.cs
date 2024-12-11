using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Domain.Cryptos
{
    public static  class CryptoErrors
    {
        public static readonly Error CryptoNotFound = new Error("Crypto", "Cryptocurrency Not Found");
        
        public static readonly Error CryptoAlreadyExist = new Error("Crypto", "Cryptocurrency Already Exists");

    }
}
