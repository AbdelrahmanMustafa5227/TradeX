using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Application.Subscriptions.Commands.AddAlert
{
    internal class AddAlertCommandHandler : ICommandHandler<AddAlertCommand>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AddAlertCommandHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(AddAlertCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetByUserIdAsync(request.UserId);
            if (subscription == null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

            var alert = Alert.Create(request.CryptoId, request.Price);

            var result = subscription.AddAlert(alert, _dateTimeProvider.UtcNow);

            if(result.IsFailure)
                return result;

            await _unitOfWork.SaveChangesAsync();
            return result;

        }
    }
}
