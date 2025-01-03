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
using TradeX.Domain.Shared;
using TradeX.Infrastructure.Persistance.Extensions;

namespace TradeX.Infrastructure.Persistance.Repositories
{
    internal class CryptoRepository : Repository<Crypto>, ICryptoRepository
    {
        public CryptoRepository(ApplicationDbContext db) : base(db)
        {

        }

        public async Task<PaginatedList<Crypto>> FilterAsync(string searchTerm, string? OrderBy = null, int page = 1, int pageSize = 0)
        {
            IQueryable<Crypto> query = DbSet.AsQueryable();

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

            return await query.GetPaginationAsync(page, pageSize);
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
