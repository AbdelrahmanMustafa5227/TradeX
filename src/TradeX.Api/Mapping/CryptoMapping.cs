using TradeX.Api.Controllers.Cryptos;
using TradeX.Application.Cryptos.Commands.CreateCrypto;
using TradeX.Application.Cryptos.Queries.GetAll;

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

        //public static GetAllCryptosQuery ToQuery(this GetAllCryptosRequest request)
        //{
        //    return new GetAllCryptosQuery
        //    (
        //        request.Search
        //        req
        //    );
        //}
    }
}
