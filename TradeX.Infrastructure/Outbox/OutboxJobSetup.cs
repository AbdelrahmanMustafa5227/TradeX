using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Infrastructure.Outbox
{
    internal class OutboxJobSetup : IConfigureOptions<QuartzOptions>
    {
        private readonly OutboxOptions _options;

        public OutboxJobSetup(IOptions<OutboxOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(QuartzOptions options)
        {
            var jobName = nameof(OutboxJob);

            options.AddJob<OutboxJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure => configure.ForJob(jobName)
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(_options.IntervalInSeconds)
            .RepeatForever()));
        }
    }
}
