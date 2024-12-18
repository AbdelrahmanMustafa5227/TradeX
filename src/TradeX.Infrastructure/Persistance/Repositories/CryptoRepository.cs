using Bookify.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Cryptos;

namespace TradeX.Infrastructure.Persistance.Repositories
{
    internal class CryptoRepository : Repository<Crypto>, ICryptoRepository
    {
        public CryptoRepository(ApplicationDbContext db) : base(db)
        {

        }

        public IQueryable<Crypto> GetAllAsync(string? searchTerm = null, string? OrderBy = null)
        {
            IQueryable<Crypto> query = DbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(x => x.Symbol.Contains(searchTerm));


            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                Expression<Func<Crypto, object>> KeySelector = OrderBy switch
                {
                    "price" => Key => Key.Price,
                    _ => Key => Key.Symbol
                };

                query = query.OrderBy(KeySelector);
            }
                
            return query;
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
