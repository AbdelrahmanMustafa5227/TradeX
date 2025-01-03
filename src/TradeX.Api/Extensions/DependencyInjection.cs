using TradeX.Api.Misc;
using TradeX.Application.Abstractions;

namespace TradeX.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation (this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
