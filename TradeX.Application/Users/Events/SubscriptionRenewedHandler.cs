using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions.Events;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Events
{
    internal class SubscriptionRenewedHandler : INotificationHandler<SubscriptionRenewed>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionRenewedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SubscriptionRenewed notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.subscription.UserId);
            if (user == null)
                return;

            var result = user.Withdraw(notification.subscription.GetPrice());
            if (result.IsFailure)
                return;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
