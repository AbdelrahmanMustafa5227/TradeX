using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Users;

namespace TradeX.Domain.Cryptos
{
    public interface ICryptoRepository
    {
        Task<Crypto?> GetByIdAsync(Guid id);

        Task<Crypto?> GetBySymbolAsync(string symbol);

        Task<bool> IsSymbolUnique(string symbol);

        void Add(Crypto crypto);
    }
}
