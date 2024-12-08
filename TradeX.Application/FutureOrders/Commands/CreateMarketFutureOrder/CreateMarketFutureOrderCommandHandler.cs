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
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Application.FutureOrders.Commands.CreateMarketFutureOrder
{
    internal class CreateMarketFutureOrderCommandHandler : ICommandHandler<CreateMarketFutureOrderCommand>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateMarketFutureOrderCommandHandler(ICryptoRepository cryptoRepository, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork, CalculateOrderDomainService calculateOrderDomainService, IDateTimeProvider dateTimeProvider)
        {
            _cryptoRepository = cryptoRepository;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _calculateOrderDomainService = calculateOrderDomainService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(CreateMarketFutureOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            if (!user.KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);

            var crypto = await _cryptoRepository.GetByIdAsync(request.CryptoId);
            if (crypto == null)
                return Result.Failure(CryptoErrors.CryptoNotFound);

            var subscription = await _subscriptionRepository.GetByUserIdAsync(request.UserId);
            if (subscription is null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);


            var order = FutureOrder.Set(user, crypto.Id, request.OrderType, request.Amount, request.EntryPrice);

            var orderDetails = _calculateOrderDomainService.CalculateOrderDetails(order, subscription, _dateTimeProvider.UtcNow);


            if (!user.CanAffordOrder(order, orderDetails))
                return Result.Failure(UserErrors.NoEnoughFunds);

            if (subscription.GetTradingVolumeLimit(_dateTimeProvider.UtcNow) < subscription.ComulativeTradingVolume24H + orderDetails.Volume)
                return Result.Failure(SubscriptionErrors.ExceededDailyTradingVolumeLimit);

            order.UpdatePricing(orderDetails);


            var openOrderResult = order.OpenOrder(_dateTimeProvider.UtcNow);
            if(!openOrderResult.IsSuccess)
                return openOrderResult;

            _orderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
