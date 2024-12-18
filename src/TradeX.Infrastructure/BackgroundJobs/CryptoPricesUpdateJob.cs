using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Shared;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Infrastructure.Abstractions;

namespace TradeX.Infrastructure.BackgroundJobs
{
    internal class CryptoPricesUpdateJob : IJob
    {
        private const decimal PriceJump = 0.001m;

        private readonly ICryptoRepository _cryptoRepository;
        private readonly IRandomNumberProvider _randomNumberProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CryptoPricesUpdateJob> _logger;

        public CryptoPricesUpdateJob(ICryptoRepository cryptoRepository, IUnitOfWork unitOfWork, IRandomNumberProvider randomNumberProvider, ILogger<CryptoPricesUpdateJob> logger)
        {
            _cryptoRepository = cryptoRepository;
            _unitOfWork = unitOfWork;
            _randomNumberProvider = randomNumberProvider;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var cryptos = _cryptoRepository.GetAllAsync();

            foreach (var crypto in cryptos)
            {
                var newPrice = crypto.Price * PriceJump * _randomNumberProvider.GetDirection() * _randomNumberProvider.GetMagnitude();
                crypto.UpdatePrice(newPrice);
                _logger.LogInformation(LogEvents.CryptoPriceUpdatedEvent,"Crypto : {Symbol} --- Price : {Price}", crypto.Symbol, Math.Round(crypto.Price,5));
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
