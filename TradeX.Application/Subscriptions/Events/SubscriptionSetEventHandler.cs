using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Subscriptions.Events;

namespace TradeX.Application.Subscriptions.Events
{
    internal class SubscriptionSetEventHandler : INotificationHandler<SubscriptionSet>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;


        public SubscriptionSetEventHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SubscriptionSet notification, CancellationToken cancellationToken)
        {
            _subscriptionRepository.Add(notification.subscription);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
