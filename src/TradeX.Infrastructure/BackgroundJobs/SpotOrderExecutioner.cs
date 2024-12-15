using MediatR;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.SpotOrders;

namespace TradeX.Infrastructure.BackgroundJobs
{
    internal class SpotOrderExecutioner : IJob
    {
        private const decimal tolerance = 0.002m;

        private readonly ISpotOrderRepository _spotOrderRepository;
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SpotOrderExecutioner(ISpotOrderRepository spotOrderRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, ICryptoRepository cryptoRepository)
        {
            _spotOrderRepository = spotOrderRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _cryptoRepository = cryptoRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var limitOrders = await _spotOrderRepository.GetAllOpenOrdersAsync();

            foreach (var order in limitOrders)
            {
                var crypto = await _cryptoRepository.GetByIdAsync(order.CryptoId);

                if (AreApproximatelyEqual(order.EntryPrice, crypto!.Price , tolerance))
                {
                    order.ExecuteLimitOrder(_dateTimeProvider.UtcNow);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private bool AreApproximatelyEqual(decimal a, decimal b ,decimal tolerance)
        {
            return Math.Abs(a - b) / a <= tolerance;
        }
    }
}
