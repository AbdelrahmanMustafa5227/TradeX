using TradeX.Domain.SpotOrders;

namespace TradeX.Api.Controllers.SpotOrders
{
    public record CreateMarketSpotOrderRequest(Guid UserId, Guid CryptoId, SpotOrderType orderType, decimal Amount)
    {

    }
}
