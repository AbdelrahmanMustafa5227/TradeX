using TradeX.Domain.FutureOrders;

namespace TradeX.Api.Controllers.FutureOrders
{
    public record PlaceMarketFutureOrderRequest(Guid UserId, Guid CryptoId, FutureOrderType OrderType, decimal Amount)
    {
    }
}