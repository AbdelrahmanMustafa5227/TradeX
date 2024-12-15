using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;

namespace TradeX.Infrastructure.BackgroundJobs
{
    internal class FutureOrderExecutioner : IJob
    {
        private const decimal tolerance = 0.002m;

        private readonly IFutureOrderRepository _futureRepository;
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public FutureOrderExecutioner(IFutureOrderRepository futureRepository, ICryptoRepository cryptoRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
        {
            _futureRepository = futureRepository;
            _cryptoRepository = cryptoRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var limitOrders = await _futureRepository.GetAllLimitOrdersAsync();
            var openOrders = await _futureRepository.GetAllOpenOrdersAsync();

            foreach (var order in limitOrders)
            {
                var crypto = await _cryptoRepository.GetByIdAsync(order.CryptoId);

                if (AreApproximatelyEqual(order.EntryPrice, crypto!.Price, tolerance))
                {
                    order.OpenOrder(_dateTimeProvider.UtcNow);
                }
            }

            foreach (var order in openOrders)
            {
                var crypto = await _cryptoRepository.GetByIdAsync(order.CryptoId);

                if (order.TakeProfitPrice != null && AreApproximatelyEqual(order.TakeProfitPrice.Value, crypto!.Price, tolerance)
                    || order.StopLossPrice != null && AreApproximatelyEqual(order.StopLossPrice.Value, crypto!.Price, tolerance))
                {
                    order.CloseOrder(crypto.Price, _dateTimeProvider.UtcNow);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private bool AreApproximatelyEqual(decimal a, decimal b, decimal tolerance)
        {
            return Math.Abs(a - b) / a <= tolerance;
        }
    }
}
