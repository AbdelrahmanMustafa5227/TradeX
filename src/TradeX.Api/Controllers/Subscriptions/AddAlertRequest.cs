namespace TradeX.Api.Controllers.Subscriptions
{
    public record AddAlertRequest(Guid UserId, Guid CryptoId, decimal Price)
    {
    }
}