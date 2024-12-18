using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Cryptos.Queries.GetAll
{
    public record GetAllCryptosQuery(string? searchTerm , string? OrderBy , int? page, int? pageSize): IQuery<List<GetAllCryptosResponse>>
    {
    }
}
