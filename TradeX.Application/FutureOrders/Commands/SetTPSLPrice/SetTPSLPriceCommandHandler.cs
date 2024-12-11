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

namespace TradeX.Application.FutureOrders.Commands.SetTPSLPrice
{
    internal class SetTPSLPriceCommandHandler : ICommandHandler<SetTPSLPriceCommand>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFutureOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SetTPSLPriceCommandHandler(ICryptoRepository cryptoRepository, IUserRepository userRepository, IFutureOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _cryptoRepository = cryptoRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(SetTPSLPriceCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(FutureOrderErrors.OrderNotFound);

            var crypto = await _cryptoRepository.GetByIdAsync(order.CryptoId);
            if (crypto == null)
                return Result.Failure(CryptoErrors.CryptoNotFound);

            var result = order.SetStopLoseTakeProfitPrice(crypto, request.TPPrice, request.SLPrice);

            if (result.IsFailure)
                return result;

            await _unitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
