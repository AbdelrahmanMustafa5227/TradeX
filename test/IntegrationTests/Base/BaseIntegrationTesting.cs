using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Infrastructure.Persistance;

namespace IntegrationTests.Base
{
    public class BaseIntegrationTesting : IClassFixture<WebAppFactory>
    {
        private readonly IServiceScope _serviceScope;
        protected readonly ISender Sender;
        protected readonly ApplicationDbContext Context;

        public BaseIntegrationTesting(WebAppFactory factory)
        {
            _serviceScope = factory.Services.CreateScope();
            Sender = _serviceScope.ServiceProvider.GetRequiredService<ISender>();
            Context = _serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }
}
