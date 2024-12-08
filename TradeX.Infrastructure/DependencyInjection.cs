using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;
using TradeX.Infrastructure.ExternalServices;
using TradeX.Infrastructure.Outbox;
using TradeX.Infrastructure.Persistance;
using TradeX.Infrastructure.Persistance.Repositories;

namespace TradeX.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ICryptoRepository, CryptoRepository>();
            services.AddScoped<ISpotOrderRepository, SpotOrderRepository>();
            services.AddScoped<IFutureOrderRepository, FutureOrderRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());


            services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
            services.AddQuartz();
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete =  true);
            services.ConfigureOptions<OutboxJobSetup>();


            return services;
        }
    }
}
