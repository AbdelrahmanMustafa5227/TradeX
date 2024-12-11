using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.DomainServices;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;

namespace TradeX.Application.SpotOrders.Commands.CreateMarketSpotOrder
{
    internal class CreateMarketSpotOrderCommandHandler : ICommandHandler<CreateMarketSpotOrderCommand>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISpotOrderRepository _spotOrderRepository;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMarketSpotOrderCommandHandler(ICryptoRepository cryptoRepository, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, ISpotOrderRepository spotOrderRepository, CalculateOrderDomainService calculateOrderDomainService, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
        {
            _cryptoRepository = cryptoRepository;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _spotOrderRepository = spotOrderRepository;
            _calculateOrderDomainService = calculateOrderDomainService;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateMarketSpotOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            if (!user.KYC_Confirmed)
                return Result.Failure(UserErrors.KYCNotConfirmed);

            var subscription = await _subscriptionRepository.GetByUserIdAsync(request.UserId);
            if (subscription is null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

            var crypto = await _cryptoRepository.GetByIdAsync(request.CryptoId);
            if (crypto is null)
                return Result.Failure(CryptoErrors.CryptoNotFound);


            var orderDetails = _calculateOrderDomainService.CalculateOrderDetails(crypto.Price ,request.Amount, subscription,
                _dateTimeProvider.UtcNow , request.orderType == SpotOrderType.Sell ,request.CryptoId);

            if (!user.CanAffordOrder(orderDetails))
                return Result.Failure(UserErrors.NoEnoughFunds);

            if (subscription.GetTradingVolumeLimit(_dateTimeProvider.UtcNow) < subscription.ComulativeTradingVolume24H + orderDetails.Volume)
                return Result.Failure(SubscriptionErrors.ExceededDailyTradingVolumeLimit);


            var order = SpotOrder.PlaceMarket(request.UserId, request.CryptoId, request.orderType, request.Amount,
                crypto.Price, orderDetails.Fees , orderDetails.Total , _dateTimeProvider.UtcNow);

            _spotOrderRepository.Add(order);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
