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
    internal class LimitSpotOrderExecutedHandler : INotificationHandler<LimitSpotOrderExecuted>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LimitSpotOrderExecutedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(LimitSpotOrderExecuted notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.Order.UserId);
            if (user == null)
                return;


            if (notification.Order.OrderType == SpotOrderType.Buy)
            {
                user.UnFreeze(notification.Order.Total);
                user.Withdraw(notification.Order.Total);
                user.AddAsset(notification.Order.CryptoId, notification.Order.Amount);
            }
            else
            {
                user.Assets.FirstOrDefault(x => x.CryptoId == notification.Order.CryptoId)!.UnFreeze(notification.Order.Amount);
                user.RemoveAsset(notification.Order.CryptoId, notification.Order.Amount);
                user.Deposit(notification.Order.Total - notification.Order.Fees);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
