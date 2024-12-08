using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.SpotOrders.Events;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Events
{
    internal class SpotOrderCreatedHandler : INotificationHandler<SpotOrderCreated>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SpotOrderCreatedHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SpotOrderCreated notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.Order.UserId);
            if (user == null)
                return;

            var result = user.AddOrder(notification.Order);
            if (result.IsFailure)
                return;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
