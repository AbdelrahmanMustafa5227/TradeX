using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Application.Abstractions.Messaging;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.RequestKycVerification
{
    internal class RequestKycCommandHandler : ICommandHandler<RequestKycCommand>
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public RequestKycCommandHandler(IEmailService emailService, IUserRepository userRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(RequestKycCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId);

            if (user == null)
            {
                return Result.Failure(UserErrors.UserNotFound);
            }

            return user.ConfirmKYC();
        }
    }
}
