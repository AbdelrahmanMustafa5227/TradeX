using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Cryptos.Commands.CreateCrypto
{
    public record CreateCryptoCommand(string Name, string Symbol, decimal Price, long TotalSupply) : ICommand
    {
    }
}
