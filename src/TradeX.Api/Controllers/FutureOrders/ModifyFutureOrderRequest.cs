namespace TradeX.Api.Controllers.FutureOrders
{
    public record ModifyFutureOrderRequest(Guid OrderId, decimal Amount)
    {
    }
}