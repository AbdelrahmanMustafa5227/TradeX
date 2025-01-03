using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;
using TradeX.Infrastructure.Persistance;

namespace TradeX.Infrastructure.BackgroundJobs
{
    internal class SubscriptionManagementJob : IJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<SubscriptionManagementJob> _logger;

        public SubscriptionManagementJob(IDateTimeProvider dateTimeProvider, ILogger<SubscriptionManagementJob> logger, ApplicationDbContext context)
        {
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
             _context.Set<Subscription>().ExecuteUpdate(p => p.SetProperty(x => x.ComulativeTradingVolume24H, 0));

            var subs = await _context.Set<Subscription>().ToListAsync();

            foreach (var sub in subs)
            {
                sub.CheckValidity(_dateTimeProvider.Today);
            }

            _logger.LogInformation("Done With Subscription Management");
            await _context.SaveChangesAsync();
        }
    }
}
