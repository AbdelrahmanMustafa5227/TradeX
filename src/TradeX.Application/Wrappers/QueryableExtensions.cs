using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Wrappers
{
    public static class QueryableExtensions
    {
        public static List<TEntity> GetPaginationAsync<TEntity>(this IQueryable<TEntity> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
