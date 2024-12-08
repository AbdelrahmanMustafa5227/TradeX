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
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ISubscriptionRepository _subscriptionRepository;


        public SetEntryPriceCommandHandler(ICryptoRepository cryptoRepository, IUserRepository userRepository, IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork, CalculateOrderDomainService calculateOrderDomainService, IDateTimeProvider dateTimeProvider, ISubscriptionRepository subscriptionRepository)
        {
            _cryptoRepository = cryptoRepository;
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
                return Result.Failure(OrderErrors.OrderNotFound);

            var user = await _userRepository.GetByIdAsync(order.UserId);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var subscription = await _subscriptionRepository.GetByUserIdAsync(order.UserId);
            if (subscription is null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

            var ModifyOrderResult = order.SetEntryPrice(request.EntryPrice);
            if (!ModifyOrderResult.IsSuccess)
                return ModifyOrderResult;

            var orderDetails = _calculateOrderDomainService.CalculateOrderDetails(order, subscription, _dateTimeProvider.UtcNow);



            bool canAfford = user.CanAffordOrder(order, orderDetails);
            if (!canAfford)
                return Result.Failure(UserErrors.NoEnoughFunds);

            order.UpdatePricing(orderDetails);

            await _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync();
            return ModifyOrderResult;
        }
    }
}
