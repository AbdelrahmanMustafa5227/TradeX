using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Application.Subscriptions.Commands.RemoveAlert
{
    internal class RemoveAlertCommandHandler : ICommandHandler<RemoveAlertCommand>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveAlertCommandHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveAlertCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetByUserIdAsync(request.UserId);
            if (subscription == null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);


            var result = subscription.RemoveAlert(request.AlertId);
            if (result.IsFailure)
                return result;

            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
