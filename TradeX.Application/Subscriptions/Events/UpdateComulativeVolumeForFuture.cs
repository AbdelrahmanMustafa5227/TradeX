using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.FutureOrders.Events;
using TradeX.Domain.Orders.Events;
using TradeX.Domain.SpotOrders.Events;
using TradeX.Domain.Subscriptions;

namespace TradeX.Application.Subscriptions.Events
{
    internal class UpdateComulativeVolumeForFuture : INotificationHandler<MarketFutureOrderPlaced>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateComulativeVolumeForFuture(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarketFutureOrderPlaced notification, CancellationToken cancellationToken)
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

    internal class UpdateComulativeVolumeForFuture2 : INotificationHandler<FutureOrderOpened>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateComulativeVolumeForFuture2(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(FutureOrderOpened notification, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetByUserIdAsync(notification.order.UserId);

            if (subscription == null)
            {
                return;
            }

            subscription.UpdateDailyComulativeVolume(notification.order.Total);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
