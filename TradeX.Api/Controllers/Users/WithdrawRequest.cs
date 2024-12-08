namespace TradeX.Api.Controllers.Users
{
    public record WithdrawRequest(Guid UserId, decimal Amount)
    {
    }
}
