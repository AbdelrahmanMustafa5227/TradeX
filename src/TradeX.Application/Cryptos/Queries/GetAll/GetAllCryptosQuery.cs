using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Shared;

namespace TradeX.Application.Cryptos.Queries.GetAll
{
    public record GetAllCryptosQuery(string searchTerm , string? OrderBy , int page, int pageSize): IQuery<PaginatedList<Crypto>>
    {

    }
}
