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
    internal class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderCommandHandler(ICryptoRepository cryptoRepository, IUserRepository userRepository, IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _cryptoRepository = cryptoRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(OrderErrors.OrderNotFound);


            var CancelOrderResult = order.CancelOrder();
            if (!CancelOrderResult.IsSuccess)
                return CancelOrderResult;


            await _orderRepository.Remove(order);
            await _unitOfWork.SaveChangesAsync();
            return CancelOrderResult;
        }
    }
}
