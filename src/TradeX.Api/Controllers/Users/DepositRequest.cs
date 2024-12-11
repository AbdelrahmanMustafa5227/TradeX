namespace TradeX.Api.Controllers.Users
{
    public record DepositRequest(Guid UserId, decimal DepositAmount)
    {
    }
}
