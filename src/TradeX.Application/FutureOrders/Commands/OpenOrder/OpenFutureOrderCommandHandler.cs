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
    internal class OpenFutureOrderCommandHandler : ICommandHandler<OpenFutureOrderCommand>
    {
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public OpenFutureOrderCommandHandler(IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(OpenFutureOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                return Result.Failure(FutureOrderErrors.OrderNotFound);


            var result = order.OpenOrder(_dateTimeProvider.UtcNow);
            if(!result.IsSuccess)
                return result;

            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
