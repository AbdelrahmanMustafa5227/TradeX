using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.Orders;
using TradeX.Domain.Users;

namespace TradeX.Application.FutureOrders.Commands.CancelOrder
{
    internal class CancelFutureOrderCommandHandler : ICommandHandler<CancelFutureOrderCommand>
    {
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelFutureOrderCommandHandler(IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CancelFutureOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(FutureOrderErrors.OrderNotFound);


            var CancelOrderResult = order.CancelOrder();
            if (!CancelOrderResult.IsSuccess)
                return CancelOrderResult;


            _orderRepository.Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return CancelOrderResult;
        }
    }
}
