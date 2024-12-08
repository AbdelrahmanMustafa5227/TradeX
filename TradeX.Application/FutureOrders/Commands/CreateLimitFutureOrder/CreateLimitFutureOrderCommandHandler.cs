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

namespace TradeX.Application.FutureOrders.Commands.CreateOrder
{
    internal class CreateLimitFutureOrderCommandHandler : ICommandHandler<CreateLimitFutureOrderCommand>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateLimitFutureOrderCommandHandler(ICryptoRepository cryptoRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IFutureOrderRepository orderRepository, CalculateOrderDomainService calculateOrderDomainService, IDateTimeProvider dateProvider, ISubscriptionRepository subscriptionRepository)
        {
            _cryptoRepository = cryptoRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _calculateOrderDomainService = calculateOrderDomainService;
            _dateTimeProvider = dateProvider;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<Result> Handle(CreateLimitFutureOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            if(!user.KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);

            var crypto = await _cryptoRepository.GetByIdAsync(request.CryptoId);
            if (crypto == null)
                return Result.Failure(CryptoErrors.CryptoNotFound);

            var subscription = await _subscriptionRepository.GetByUserIdAsync(request.UserId);
            if (subscription is null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);


            var order = FutureOrder.Set(user, crypto.Id, request.OrderType, request.Amount, request.EntryPrice);

            var orderDetails = _calculateOrderDomainService.CalculateOrderDetails(order, subscription, _dateTimeProvider.UtcNow);


            if(!user.CanAffordOrder(order, orderDetails))
                return Result.Failure(UserErrors.NoEnoughFunds);

            if (subscription.GetTradingVolumeLimit(_dateTimeProvider.UtcNow) < subscription.ComulativeTradingVolume24H + orderDetails.Volume)
                return Result.Failure(SubscriptionErrors.ExceededDailyTradingVolumeLimit);

            order.UpdatePricing(orderDetails);

            _orderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
