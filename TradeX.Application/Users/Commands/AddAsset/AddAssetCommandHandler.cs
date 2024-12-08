using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.AddAsset
{
    internal class AddAssetCommandHandler : ICommandHandler<AddAssetCommand>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddAssetCommandHandler(ICryptoRepository cryptoRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _cryptoRepository = cryptoRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(AddAssetCommand request, CancellationToken cancellationToken)
        {
            var crypto = await _cryptoRepository.GetByIdAsync(request.CryptoId);
            if (crypto == null)
                return Result.Failure(CryptoErrors.CryptoNotFound);

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            user.AddAsset(request.CryptoId, request.Amount);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
