using TradeX.Domain.FutureOrders;

namespace TradeX.Api.Controllers.FutureOrders
{
    public record PlaceLimitFutureOrderRequest(Guid UserId, Guid CryptoId, FutureOrderType OrderType, decimal EntryPrice, decimal Amount)
    {
    }
}