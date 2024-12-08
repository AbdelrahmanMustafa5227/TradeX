using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Application.Subscriptions.Commands.RenewSubscription
{
    internal class RenewSubscriptionCommandHandler : ICommandHandler<RenewSubscriptionCommand>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RenewSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IUserRepository userRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(RenewSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionRepository.GetByUserIdAsync(request.UserId);
            if (subscription == null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var price = subscription.GetPrice();

            if (user.balance.AvailableBalance < price)
                return Result.Failure(UserErrors.NoEnoughFunds);


            var result = subscription.RenewSubscription(_dateTimeProvider.Today);

            if (result.IsFailure)
                return result;

            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
