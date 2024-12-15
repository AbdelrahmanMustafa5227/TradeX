namespace TradeX.Api.Controllers.Users
{
    public record RefreshRequest(string AccessToken ,Guid RefreshToken)
    {
    }
}
