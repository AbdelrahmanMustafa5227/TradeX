using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.FutureOrders.Events;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Events
{
    internal class LimitFutureOrderPlacedHandler : INotificationHandler<LimitFutureOrderPlaced>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LimitFutureOrderPlacedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(LimitFutureOrderPlaced notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.Order.UserId);
            if (user == null)
                return;

            var result = user.AddOrder(notification.Order);
            if (result.IsFailure)
                return;

            user.Freeze(notification.Order.Total);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
