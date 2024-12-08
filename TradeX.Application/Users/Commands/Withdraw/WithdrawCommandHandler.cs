using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.Deposit
{
    internal class WithdrawCommandHandler : ICommandHandler<WithdrawCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WithdrawCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);


            var result = user.Withdraw(request.Amount);

            if(result.IsFailure)
                return result;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
