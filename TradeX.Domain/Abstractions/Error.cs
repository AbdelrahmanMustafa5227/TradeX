namespace TradeX.Domain.Abstractions
{
    public record Error (string code , string message)
    {
        public static readonly Error Null = new Error(string.Empty, "Null value was provided");
        public static readonly Error None = new Error(string.Empty, string.Empty);
    }
}
