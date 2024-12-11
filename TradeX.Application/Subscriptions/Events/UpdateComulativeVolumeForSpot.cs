using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.FutureOrders.Events;
using TradeX.Domain.SpotOrders.Events;
using TradeX.Domain.Subscriptions;

namespace TradeX.Application.Subscriptions.Events
{
    internal class UpdateComulativeVolumeForSpot : INotificationHandler<MarketSpotOrderExecuted>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateComulativeVolumeForSpot(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarketSpotOrderExecuted notification, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetByUserIdAsync(notification.Order.UserId);

            if (subscription == null)
            {
                return;
            }

            subscription.UpdateDailyComulativeVolume(notification.Order.Total);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
