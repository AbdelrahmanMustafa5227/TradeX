using TradeX.Api.Controllers.Cryptos;
using TradeX.Application.Cryptos.Commands.CreateCrypto;

namespace TradeX.Api.Mapping
{
    public static class CryptoMapping
    {
        public static CreateCryptoCommand ToCommand(this CreateCryptoRequest request)
        {
            return new CreateCryptoCommand
            (
                request.Name,
                request.Symbol,
                request.Price,
                request.TotalSupply
            );
        }
    }
}
