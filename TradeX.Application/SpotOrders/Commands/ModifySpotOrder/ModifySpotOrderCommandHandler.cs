﻿using System;
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

namespace TradeX.Application.SpotOrders.Commands.ModifySpotOrder
{
    internal class ModifySpotOrderCommandHandler : ICommandHandler<ModifySpotOrderCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISpotOrderRepository _spotOrderRepository;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public ModifySpotOrderCommandHandler(IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, ISpotOrderRepository spotOrderRepository, CalculateOrderDomainService calculateOrderDomainService, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _spotOrderRepository = spotOrderRepository;
            _calculateOrderDomainService = calculateOrderDomainService;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ModifySpotOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _spotOrderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(OrderErrors.OrderNotFound);

            var subscription = await _subscriptionRepository.GetByUserIdAsync(order.UserId);
            if (subscription is null)
                return Result.Failure(SubscriptionErrors.SubscriptionNotFound);

            var user = await _userRepository.GetByIdAsync(order.UserId);
            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            var oldAmount = order.Amount;

            var ModifyOrderResult = order.ModifyOrder(request.orderType, request.Amount);
            if(!ModifyOrderResult.IsSuccess)
                return ModifyOrderResult;


            var orderDetails = _calculateOrderDomainService.CalculateOrderDetails(order, subscription, _dateTimeProvider.UtcNow);


            if(!user.CanAffordOrder(order, orderDetails))
                return Result.Failure(UserErrors.NoEnoughFunds);

            if (subscription.GetTradingVolumeLimit(_dateTimeProvider.UtcNow) < subscription.ComulativeTradingVolume24H + orderDetails.Volume)
                return Result.Failure(SubscriptionErrors.ExceededDailyTradingVolumeLimit);

            order.UpdatePricing(oldAmount, orderDetails);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}