namespace TradeX.Api.Controllers.FutureOrders
{
    public record ChangeTPSLRequest(Guid OrderId, decimal TPPrice, decimal SLPrice)
    {
    }
}