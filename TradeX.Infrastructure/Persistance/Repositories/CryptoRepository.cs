using Bookify.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Cryptos;

namespace TradeX.Infrastructure.Persistance.Repositories
{
    internal class CryptoRepository : Repository<Crypto> , ICryptoRepository
    {
        public CryptoRepository(ApplicationDbContext db) : base(db)
        {
            
        }

        public async Task<List<Crypto>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Crypto?> GetBySymbolAsync(string symbol)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Symbol == symbol);
        }

        public async Task<bool> IsSymbolUnique(string symbol)
        {
            return await DbSet.AllAsync(x => x.Symbol != symbol);
        }
    }
}
