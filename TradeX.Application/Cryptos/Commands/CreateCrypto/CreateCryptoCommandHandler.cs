using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;

namespace TradeX.Application.Cryptos.Commands.CreateCrypto
{
    internal class CreateCryptoCommandHandler : ICommandHandler<CreateCryptoCommand>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCryptoCommandHandler(ICryptoRepository cryptoRepository, IUnitOfWork unitOfWork)
        {
            _cryptoRepository = cryptoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateCryptoCommand request, CancellationToken cancellationToken)
        {
            bool isUnique = await _cryptoRepository.IsSymbolUnique(request.Symbol);

            if (!isUnique)
                return Result.Failure(CryptoErrors.CryptoAlreadyExist);

            var crypto = Crypto.Create(request.Name, request.Symbol, request.Price, request.TotalSupply);
            _cryptoRepository.Add(crypto);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
