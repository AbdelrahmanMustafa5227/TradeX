using TradeX.Domain.SpotOrders;

namespace TradeX.Api.Controllers.SpotOrders
{
    public record CreateLimitSpotOrderRequest(Guid UserId, Guid CryptoId, SpotOrderType orderType, decimal Amount, decimal EntryPrice)
    {

    }
}