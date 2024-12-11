using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.ConfirmKYC
{
    internal class Confirm_KYC_CommandHandler : ICommandHandler<Confirm_KYC_Command>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Confirm_KYC_CommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(Confirm_KYC_Command request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null) 
                return Result.Failure(UserErrors.UserNotFound);

            var result = user.ConfirmKYC();

            if (result.IsFailure)
                return result;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
