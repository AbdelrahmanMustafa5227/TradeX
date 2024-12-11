using TradeX.Domain.SpotOrders;

namespace TradeX.Api.Controllers.SpotOrders
{
    public record ModifyLimitOrderRequest(Guid OrderId, SpotOrderType orderType, decimal Amount)
    {
    }
}