﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using TradeX.Application.Abstractions.Authentication;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Cryptos;
using TradeX.Domain.FutureOrders;
using TradeX.Domain.SpotOrders;
using TradeX.Domain.Subscriptions;
using TradeX.Domain.Users;
using TradeX.Infrastructure.Abstractions;
using TradeX.Infrastructure.Authentication;
using TradeX.Infrastructure.BackgroundJobs;
using TradeX.Infrastructure.BackgroundJobs.Outbox;
using TradeX.Infrastructure.ExternalServices;
using TradeX.Infrastructure.Persistance;
using TradeX.Infrastructure.Persistance.Repositories;
using TradeX.Infrastructure.Services;

namespace TradeX.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IRandomNumberProvider, RandomNumberProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ICryptoRepository, CryptoRepository>();
            services.AddScoped<ISpotOrderRepository, SpotOrderRepository>();
            services.AddScoped<IFutureOrderRepository, FutureOrderRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());


            services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();
            services.ConfigureOptions<AuthenticationOptionsSetup>();
            services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();


            services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
            services.AddQuartz();
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
            services.ConfigureOptions<QuartzSetup>();


            return services;
        }
    }
}
