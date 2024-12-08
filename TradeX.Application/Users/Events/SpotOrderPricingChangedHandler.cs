using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.SpotOrders.Events;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Events
{
    internal class SpotOrderPricingChangedHandler : INotificationHandler<SpotOrderPricingChanged>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SpotOrderPricingChangedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SpotOrderPricingChanged notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.Order.UserId);
            if (user == null)
                return;

            if(!user.SpotOrders.Contains(notification.Order.Id))
                user.AddOrder(notification.Order);

            if (notification.Order.OrderType == SpotOrderType.Buy)
            {
                user.UnFreeze(notification.oldPrice);
                user.Freeze(notification.Order.Total);
            }
            else
            {
                var asset = user.Assets.FirstOrDefault(x => x.CryptoId == notification.Order.CryptoId);
                asset!.UnFreeze(notification.oldAmount);
                asset!.Freeze(notification.Order.Amount);
            }
            
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
