using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;

namespace TradeX.Infrastructure.ExternalServices
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateOnly Today => DateOnly.FromDateTime(UtcNow);
    }
}
