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

namespace TradeX.Infrastructure.BackgroundJobs
{
    internal class SubscriptionManagementJob : IJob
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<SubscriptionManagementJob> _logger;

        public SubscriptionManagementJob(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, ILogger<SubscriptionManagementJob> logger)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _subscriptionRepository.ZeriongAll();

            var subs = await _subscriptionRepository.GetAllAsync();

            foreach (var sub in subs)
            {
                sub.CheckValidity(_dateTimeProvider.Today);
            }

            _logger.LogInformation("Done With Subscription Management");
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
