using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Events
{
    internal class FutureOrderClosedHandler : INotificationHandler<FutureOrderClosed>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FutureOrderClosedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(FutureOrderClosed notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.order.UserId);
            if (user == null)
                return;


            var diff = (decimal)(notification.order.ExitPrice - notification.order.EntryPrice)!;

            if (diff > 0 && notification.order.Type == FutureOrderType.Long
                || diff < 0 && notification.order.Type == FutureOrderType.Short)
            {
                user.Deposit(Math.Abs(diff) * notification.order.Amount);
            }
            else
            {
                user.Withdraw(Math.Abs(diff) * notification.order.Amount);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
