using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.SetSubsciption
{
    internal class SetSubscriptionCommandHandler : ICommandHandler<SetSubscriptionCommand,Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public SetSubscriptionCommandHandler(IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(SetSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
            {
                return Result.Failure<Guid>(UserErrors.UserNotFound);
            }

            var subscription = Subscription.Create(user.Id, request.Tier, request.Plan, _dateTimeProvider.Today);

            var result = user.SetSubscription(subscription);

            if (result.IsFailure)
                return Result.Failure<Guid>(result.Error!);

            await _unitOfWork.SaveChangesAsync();
            return subscription.Id;
        }
    }
}
