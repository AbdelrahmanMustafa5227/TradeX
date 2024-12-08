namespace TradeX.Api.Controllers.Subscriptions
{
    public record RemoveAlertRequest(Guid UserId , Guid AlertId)
    {
    }
}