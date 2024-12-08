using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Users.Commands.AddAsset;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.RemoveAsset
{
    internal class RemoveAssetCommandHandler : ICommandHandler<RemoveAssetCommand>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveAssetCommandHandler(ICryptoRepository cryptoRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _cryptoRepository = cryptoRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(RemoveAssetCommand request, CancellationToken cancellationToken)
        {
            var crypto = await _cryptoRepository.GetByIdAsync(request.CryptoId);
            if (crypto == null)
                return Result.Failure(CryptoErrors.CryptoNotFound);

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var result = user.RemoveAsset(request.CryptoId ,request.Amount);

            if (!result.IsSuccess)
                return result;

            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
