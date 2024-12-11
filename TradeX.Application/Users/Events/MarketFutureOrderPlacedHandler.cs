using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.FutureOrders.Events;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Events
{
    internal class MarketFutureOrderPlacedHandler : INotificationHandler<MarketFutureOrderPlaced>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarketFutureOrderPlacedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarketFutureOrderPlaced notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.Order.UserId);
            if (user == null)
                return;

            user.AddOrder(notification.Order);
            user.Withdraw(notification.Order.Total);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
