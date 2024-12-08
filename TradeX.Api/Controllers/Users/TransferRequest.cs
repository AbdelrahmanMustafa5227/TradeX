namespace TradeX.Api.Controllers.Users
{
    public record TransferRequest(Guid SenderId, Guid RecepientId, decimal Amount)
    {
    }
}
