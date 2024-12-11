namespace TradeX.Infrastructure.Abstractions
{
    internal interface IRandomNumberProvider
    {
        int GetDirection();
        decimal GetMagnitude();
    }
}