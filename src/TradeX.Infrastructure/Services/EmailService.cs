using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;

namespace TradeX.Infrastructure.ExternalServices
{
    internal class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string title, string body)
        {
            return Task.CompletedTask;
        }
    }
}
