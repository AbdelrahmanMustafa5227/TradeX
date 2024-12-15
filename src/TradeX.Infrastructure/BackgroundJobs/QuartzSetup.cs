using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Infrastructure.BackgroundJobs.Outbox;

namespace TradeX.Infrastructure.BackgroundJobs
{
    internal class QuartzSetup : IConfigureOptions<QuartzOptions>
    {
        private readonly OutboxOptions _options;

        public QuartzSetup(IOptions<OutboxOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(QuartzOptions options)
        {
            options.AddJob<CryptoPricesUpdateJob>(configure => configure.WithIdentity(nameof(CryptoPricesUpdateJob)))
            .AddTrigger(configure => configure.ForJob(nameof(CryptoPricesUpdateJob))
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(2).RepeatForever()));

            options.AddJob<SpotOrderExecutioner>(configure => configure.WithIdentity(nameof(SpotOrderExecutioner)))
            .AddTrigger(configure => configure.ForJob(nameof(SpotOrderExecutioner))
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(2).RepeatForever()));

            options.AddJob<FutureOrderExecutioner>(configure => configure.WithIdentity(nameof(FutureOrderExecutioner)))
            .AddTrigger(configure => configure.ForJob(nameof(FutureOrderExecutioner))
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(2).RepeatForever()));


            options.AddJob<OutboxJob>(configure => configure.WithIdentity(nameof(OutboxJob)))
            .AddTrigger(configure => configure.ForJob(nameof(OutboxJob))
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(_options.IntervalInSeconds)
            .RepeatForever()));


            options.AddJob<SubscriptionManagementJob>(configure => configure.WithIdentity(nameof(SubscriptionManagementJob)))
            .AddTrigger(configure => configure.ForJob(nameof(SubscriptionManagementJob))
            .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(24).RepeatForever()));

            



        }
    }
}
