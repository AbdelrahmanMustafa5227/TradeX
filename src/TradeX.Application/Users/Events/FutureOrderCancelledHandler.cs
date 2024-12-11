using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.SpotOrders.Events;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Events
{
    internal class FutureOrderCancelledHandler : INotificationHandler<FutureOrderCancelled>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FutureOrderCancelledHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(FutureOrderCancelled notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.Order.UserId);
            if (user == null)
                return;

            user.RemoveOrder(notification.Order);
            user.UnFreeze(notification.Order.Total);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
