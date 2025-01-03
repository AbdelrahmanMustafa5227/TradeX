using Microsoft.EntityFrameworkCore;
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
using TradeX.Infrastructure.Persistance;

namespace TradeX.Infrastructure.BackgroundJobs
{
    internal class CryptoPricesUpdateJob : IJob
    {
        private const decimal PriceJump = 0.001m;

        private readonly ApplicationDbContext _context;
        private readonly IRandomNumberProvider _randomNumberProvider;
        private readonly ILogger<CryptoPricesUpdateJob> _logger;

        public CryptoPricesUpdateJob(ApplicationDbContext context, IRandomNumberProvider randomNumberProvider, ILogger<CryptoPricesUpdateJob> logger)
        {
            _context = context;
            _randomNumberProvider = randomNumberProvider;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var cryptos = await _context.Set<Crypto>().ToListAsync();

            foreach (var crypto in cryptos)
            {
                var newPrice = crypto.Price * PriceJump * _randomNumberProvider.GetDirection() * _randomNumberProvider.GetMagnitude();
                crypto.UpdatePrice(newPrice);


                _logger.LogInformation(
                LogEvents.CryptoPriceUpdatedEvent,
                "Crypto : {Symbol} --- Price : ${Price:F4}",
                crypto.Symbol, crypto.Price);

            }

            await _context.SaveChangesAsync();
        }
    }
}
