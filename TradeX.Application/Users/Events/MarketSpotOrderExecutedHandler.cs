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
    internal class MarketSpotOrderExecutedHandler : INotificationHandler<MarketSpotOrderExecuted>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarketSpotOrderExecutedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarketSpotOrderExecuted notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.Order.UserId);
            if (user == null)
                return;

            user.AddOrder(notification.Order);


            if(notification.Order.OrderType == SpotOrderType.Buy)
            {
                user.Withdraw(notification.Order.Total);
                user.AddAsset(notification.Order.CryptoId, notification.Order.Amount);
            }
            else
            {
                user.Deposit(notification.Order.Total - notification.Order.Fees);
                user.RemoveAsset(notification.Order.CryptoId, notification.Order.Amount);
            }
                
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
