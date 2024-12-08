using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.Transfer
{
    internal class TransferCommandHandler : ICommandHandler<TransferCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var sender = await _userRepository.GetByIdAsync(request.SenderId);
            var recepient = await _userRepository.GetByIdAsync(request.RecepientId);

            if(sender is null || recepient is null) 
                return Result.Failure(UserErrors.UserNotFound);

            var result = sender.Transfer(request.Amount, recepient);

            if (result.IsSuccess)
            {
                await _unitOfWork.SaveChangesAsync();
            }

            return result;
        }
    }
}
