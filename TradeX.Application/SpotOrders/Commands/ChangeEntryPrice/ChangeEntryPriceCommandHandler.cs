using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.DomainServices;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Application.SpotOrders.Commands.ChangeEntryPrice
{
    internal class ChangeEntryPriceCommandHandler : ICommandHandler<ChangeEntryPriceCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISpotOrderRepository _spotOrderRepository;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeEntryPriceCommandHandler(ISpotOrderRepository spotOrderRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, CalculateOrderDomainService calculateOrderDomainService, IDateTimeProvider dateTimeProvider)
        {
            _spotOrderRepository = spotOrderRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _calculateOrderDomainService = calculateOrderDomainService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(ChangeEntryPriceCommand request, CancellationToken cancellationToken)
        {
            var order = await _spotOrderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(SpotOrderErrors.SpotOrderNotFound);

            var subscription = await _subscriptionRepository.GetByUserIdAsync(order.UserId);
            if (subscription is null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

            var user = await _userRepository.GetByIdAsync(order.UserId);
            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);


            var orderDetails = _calculateOrderDomainService.CalculateOrderDetails(request.EntryPrice, order.Amount,
                                subscription, _dateTimeProvider.UtcNow, order.OrderType == SpotOrderType.Sell, order.CryptoId);


            if (!user.CanAffordOrder(orderDetails))
                return Result.Failure(UserErrors.NoEnoughFunds);

            if (subscription.GetTradingVolumeLimit(_dateTimeProvider.UtcNow) < subscription.ComulativeTradingVolume24H + orderDetails.Volume)
                return Result.Failure(SubscriptionErrors.ExceededDailyTradingVolumeLimit);


            var SetEntryPriceResult = order.SetEntryPrice(request.EntryPrice, orderDetails);
            if (SetEntryPriceResult.IsFailure)
                return SetEntryPriceResult;

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
