using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.SpotOrders;

namespace TradeX.Application.SpotOrders.Commands.ExecuteLimitOrder
{
    internal class ExecuteLimitOrderCommandHandler : ICommandHandler<ExecuteLimitOrderCommand>
    {
        private readonly ISpotOrderRepository _spotOrderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ExecuteLimitOrderCommandHandler(ISpotOrderRepository spotOrderRepository, IUnitOfWork unitOfWork, IDateTimeProvider timeProvider)
        {
            _spotOrderRepository = spotOrderRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = timeProvider;
        }

        public async Task<Result> Handle(ExecuteLimitOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _spotOrderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                return Result.Failure(SpotOrderErrors.SpotOrderNotFound);

            order.ExecuteLimitOrder(_dateTimeProvider.UtcNow);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
