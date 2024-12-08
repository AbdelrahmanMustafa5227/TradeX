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

namespace TradeX.Application.FutureOrders.Commands.ModifyOrder
{
    internal class ModifyOrderCommandHandler : ICommandHandler<ModifyOrderCommand>
    {
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ModifyOrderCommandHandler(IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, CalculateOrderDomainService calculateOrderDomainService, IDateTimeProvider dateTimeProvider)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _calculateOrderDomainService = calculateOrderDomainService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(ModifyOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(OrderErrors.OrderNotFound);

            var user = await _userRepository.GetByIdAsync(order.UserId);
            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            var subscription = await _subscriptionRepository.GetByUserIdAsync(order.UserId);
            if (subscription is null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

            var ModifyOrderResult = order.ModifyOrder(request.OrderType, request.Amount);
            if (!ModifyOrderResult.IsSuccess)
                return ModifyOrderResult;


            var orderDetails = _calculateOrderDomainService.CalculateOrderDetails(order, subscription, _dateTimeProvider.UtcNow);


            bool canAfford = user.CanAffordOrder(order,orderDetails);
            if (!canAfford)
                return Result.Failure(UserErrors.NoEnoughFunds);

            order.UpdatePricing(orderDetails);

            await _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync();
            return ModifyOrderResult;
        }
    }
}
