using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Events
{
    internal class FutureOrderOpenedHandler : INotificationHandler<FutureOrderOpened>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FutureOrderOpenedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(FutureOrderOpened notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.order.UserId);
            if (user == null)
                return;

            user.UnFreeze(notification.order.Total);
            user.Withdraw(notification.order.Total);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
