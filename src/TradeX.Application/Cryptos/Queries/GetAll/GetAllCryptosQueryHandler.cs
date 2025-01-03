using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Shared;

namespace TradeX.Application.Cryptos.Queries.GetAll
{
    internal class GetAllCryptosQueryHandler : IQueryHandler<GetAllCryptosQuery, PaginatedList<Crypto>>
    {
        private readonly ICryptoRepository _cryptoRepository;

        public GetAllCryptosQueryHandler(ICryptoRepository cryptoRepository)
        {
            _cryptoRepository = cryptoRepository;
        }

        public async Task<Result<PaginatedList<Crypto>>> Handle(GetAllCryptosQuery request, CancellationToken cancellationToken)
        {
            var cryptos = await _cryptoRepository.FilterAsync(request.searchTerm, request.OrderBy , request.page , request.pageSize);

           
            return cryptos;
        }
    }
}
