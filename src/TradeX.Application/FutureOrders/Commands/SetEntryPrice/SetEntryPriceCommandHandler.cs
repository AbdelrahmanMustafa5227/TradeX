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
using TradeX.Domain.Orders;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Application.FutureOrders.Commands.SetEntryPrice
{
    internal class SetEntryPriceCommandHandler : ICommandHandler<SetEntryPriceCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ISubscriptionRepository _subscriptionRepository;


        public SetEntryPriceCommandHandler(IUserRepository userRepository, IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork, CalculateOrderDomainService calculateOrderDomainService, IDateTimeProvider dateTimeProvider, ISubscriptionRepository subscriptionRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _calculateOrderDomainService = calculateOrderDomainService;
            _dateTimeProvider = dateTimeProvider;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<Result> Handle(SetEntryPriceCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(FutureOrderErrors.OrderNotFound);

            var user = await _userRepository.GetByIdAsync(order.UserId);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var subscription = await _subscriptionRepository.GetByUserIdAsync(order.UserId);
            if (subscription is null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

            
            var orderDetails = _calculateOrderDomainService.CalculateOrderDetails(request.EntryPrice , order.Amount, subscription, _dateTimeProvider.UtcNow);


            if (!user.CanAffordOrder(orderDetails))
                return Result.Failure(UserErrors.NoEnoughFunds);

            if (subscription.GetTradingVolumeLimit(_dateTimeProvider.UtcNow) < subscription.ComulativeTradingVolume24H + orderDetails.Volume)
                return Result.Failure(SubscriptionErrors.ExceededDailyTradingVolumeLimit);


            var SetEntryPriceResult = order.SetEntryPrice(request.EntryPrice , orderDetails);
            if (SetEntryPriceResult.IsFailure)
                return SetEntryPriceResult;


            await _unitOfWork.SaveChangesAsync();
            return SetEntryPriceResult;
        }
    }
}
