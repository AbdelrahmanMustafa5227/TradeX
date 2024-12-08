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

namespace TradeX.Application.SpotOrders.Commands.CancelSpotOrder
{
    internal class CancelSpotOrderCommandHandler : ICommandHandler<CancelSpotOrderCommand>
    {
        private readonly ISpotOrderRepository _spotOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelSpotOrderCommandHandler(ISpotOrderRepository spotOrderRepository, IUnitOfWork unitOfWork)
        {
            _spotOrderRepository = spotOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CancelSpotOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _spotOrderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(OrderErrors.OrderNotFound);

            var cancelOrderResult = order.CancelOrder();
            if (!cancelOrderResult.IsSuccess)
                return cancelOrderResult;

            _spotOrderRepository.Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
