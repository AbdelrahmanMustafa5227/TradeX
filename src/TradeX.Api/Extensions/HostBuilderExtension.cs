using Serilog;
using TradeX.Domain.Cryptos;

namespace TradeX.Api.Extensions
{
    public static class HostBuilderExtension
    {
        public static void ConfigureLogger(this ConfigureHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig.ReadFrom.Configuration(context.Configuration);
                loggerConfig.Destructure.ByTransforming<Crypto>(x => new { x.Price, x.Symbol });

            });
        }
    }
}
