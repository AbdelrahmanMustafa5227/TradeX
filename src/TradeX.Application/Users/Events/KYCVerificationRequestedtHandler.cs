using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;
using TradeX.Domain.Users.Events;

namespace TradeX.Application.Users.Events
{
    internal class KYCVerificationRequestedtHandler : INotificationHandler<KYCVerificationRequested>
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public KYCVerificationRequestedtHandler(IEmailService emailService, IUserRepository userRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task Handle(KYCVerificationRequested notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.UserId);

            if (user == null)
            {
                return;
            }

            await _emailService.SendEmailAsync(user.Email, "KYC Request is Being Processed", "It Can take up to 3 business days");
        }
    }
}
