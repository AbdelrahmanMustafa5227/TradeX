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

namespace TradeX.Application.FutureOrders.Commands.OpenOrder
{
    internal class OpenOrderCommandHandler : ICommandHandler<OpenOrderCommand>
    {
        private readonly IFutureOrderRepository _orderRepository;
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly CalculateOrderDomainService _calculateOrderDomainService;

        public OpenOrderCommandHandler(IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, CalculateOrderDomainService calculateOrderDomainService, ICryptoRepository cryptoRepository, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _calculateOrderDomainService = calculateOrderDomainService;
            _cryptoRepository = cryptoRepository;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<Result> Handle(OpenOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                return Result.Failure(OrderErrors.OrderNotFound);


            var result = order.OpenOrder(_dateTimeProvider.UtcNow);
            if(!result.IsSuccess)
                return result;

            await _orderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
