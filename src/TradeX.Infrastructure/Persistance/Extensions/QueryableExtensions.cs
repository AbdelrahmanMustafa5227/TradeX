using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Shared;

namespace TradeX.Infrastructure.Persistance.Extensions
{
    public static class QueryableExtensions
    {
        private const int defaultPage = 1;
        private const int defaultPageSize = 4;

        public async static Task<PaginatedList<TEntity>> GetPaginationAsync<TEntity>(this IQueryable<TEntity> query, int page, int pageSize)
        {
            if (page < 1)
                page = defaultPage;

            if (pageSize < 1)
                pageSize = defaultPageSize;

            int totalCount = query.Count();

            return PaginatedList<TEntity>.CreateAsync(
                await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                page,
                pageSize,
                totalCount);

        }
    }
}
