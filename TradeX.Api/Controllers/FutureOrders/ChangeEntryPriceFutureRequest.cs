namespace TradeX.Api.Controllers.FutureOrders
{
    public record ChangeEntryPriceFutureRequest(Guid OrderId, decimal EntryPrice)
    {
    }
}