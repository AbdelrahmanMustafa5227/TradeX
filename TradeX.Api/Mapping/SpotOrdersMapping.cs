using TradeX.Api.Controllers.SpotOrders;
using TradeX.Application.SpotOrders.Commands.CancelSpotOrder;
using TradeX.Application.SpotOrders.Commands.ChangeEntryPrice;
using TradeX.Application.SpotOrders.Commands.CreateLimitSpotOrder;
using TradeX.Application.SpotOrders.Commands.CreateMarketSpotOrder;
using TradeX.Application.SpotOrders.Commands.ModifySpotOrder;

namespace TradeX.Api.Mapping
{
    public static class SpotOrdersMapping
    {
        public static CreateMarketSpotOrderCommand ToCommand(this CreateMarketSpotOrderRequest request)
        {
            return new CreateMarketSpotOrderCommand
            (
                request.UserId,
                request.CryptoId,
                request.orderType,
                request.Amount
            );
        }

        public static CreateLimitSpotOrderCommand ToCommand(this CreateLimitSpotOrderRequest request)
        {
            return new CreateLimitSpotOrderCommand
            (
                request.UserId,
                request.CryptoId,
                request.orderType,
                request.Amount,
                request.EntryPrice
            );
        }

        public static CancelSpotOrderCommand ToCommand(this CancelLimitSpotOrderRequest request)
        {
            return new CancelSpotOrderCommand
            (
                request.OrderId
            );
        }

        public static ChangeEntryPriceCommand ToCommand(this ChangeEntryPriceRequest request)
        {
            return new ChangeEntryPriceCommand
            (
                request.OrderId,
                request.EntryPrice
            );
        }

        public static ModifySpotOrderCommand ToCommand(this ModifyLimitOrderRequest request)
        {
            return new ModifySpotOrderCommand
            (
                request.OrderId,
                request.orderType,
                request.Amount
            );
        }


    }
}
