using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Wrappers;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;

namespace TradeX.Application.Cryptos.Queries.GetAll
{
    internal class GetAllCryptosQueryHandler : IQueryHandler<GetAllCryptosQuery, List<GetAllCryptosResponse>>
    {
        private readonly ICryptoRepository _cryptoRepository;

        public GetAllCryptosQueryHandler(ICryptoRepository cryptoRepository)
        {
            _cryptoRepository = cryptoRepository;
        }

        public async Task<Result<List<GetAllCryptosResponse>>> Handle(GetAllCryptosQuery request, CancellationToken cancellationToken)
        {
            var cryptos = _cryptoRepository.GetAllAsync(request.searchTerm , request.OrderBy)
                .GetPaginationAsync(request.page ?? 1 , request.pageSize ?? 3);

            var response = new List<GetAllCryptosResponse>();

            foreach (var crypto in cryptos)
            {
                response.Add(new GetAllCryptosResponse(crypto.Name, crypto.Symbol, crypto.Price));
            }

            await Task.CompletedTask;
            return response;
        }
    }
}
