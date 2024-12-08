using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Infrastructure.Outbox
{
    internal class OutboxOptions
    {
        public const string SectionName = "OutboxOptions";

        public int IntervalInSeconds { get; init; }

        public int BatchSize { get; init; }
    }
}
