using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Domain.Shared
{
    public class PaginatedList<T> 
    {
        private PaginatedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public bool HasNextPage => Page * PageSize < TotalCount;

        public bool HasPrevPage => Page > 1;

        public static PaginatedList<T> CreateAsync(List<T> query, int page, int pageSize , int totalCount)
        {
            return new PaginatedList<T>(query, page, pageSize, totalCount);
        }
    }
}
