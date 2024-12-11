namespace TradeX.Api.Controllers.Cryptos
{
    public record CreateCryptoRequest(string Name, string Symbol, decimal Price, long TotalSupply)
    {
    }
}
