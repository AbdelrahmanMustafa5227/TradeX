namespace TradeX.Api.Controllers.SpotOrders
{
    public record ChangeEntryPriceRequest(Guid OrderId, decimal EntryPrice)
    {
    }
}