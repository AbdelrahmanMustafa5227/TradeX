using TradeX.Api.Controllers.FutureOrders;
using TradeX.Application.FutureOrders.Commands.CancelOrder;
using TradeX.Application.FutureOrders.Commands.CreateMarketFutureOrder;
using TradeX.Application.FutureOrders.Commands.CreateOrder;
using TradeX.Application.FutureOrders.Commands.ModifyOrder;
using TradeX.Application.FutureOrders.Commands.OpenOrder;
using TradeX.Application.FutureOrders.Commands.SetEntryPrice;
using TradeX.Application.FutureOrders.Commands.SetTPSLPrice;

namespace TradeX.Api.Mapping
{
    public static class FutureOrdersMapping
    {
        public static CreateMarketFutureOrderCommand ToCommand (this PlaceMarketFutureOrderRequest request)
        {
            return new CreateMarketFutureOrderCommand
            (
                request.UserId,
                request.CryptoId,
                request.OrderType,
                request.Amount
            );
        }

        public static CreateLimitFutureOrderCommand ToCommand(this PlaceLimitFutureOrderRequest request)
        {
            return new CreateLimitFutureOrderCommand
            (
                request.UserId,
                request.CryptoId,
                request.OrderType,
                request.EntryPrice,
                request.Amount
            );
        }

        public static SetEntryPriceCommand ToCommand(this ChangeEntryPriceFutureRequest request)
        {
            return new SetEntryPriceCommand
            (
                request.OrderId,
                request.EntryPrice
            );
        }

        public static SetTPSLPriceCommand ToCommand(this ChangeTPSLRequest request)
        {
            return new SetTPSLPriceCommand
            (
                request.OrderId,
                request.TPPrice,
                request.SLPrice
            );
        }

        public static ModifyFutureOrderCommand ToCommand(this ModifyFutureOrderRequest request)
        {
            return new ModifyFutureOrderCommand
            (
                request.OrderId,
                request.Amount
            );
        }

        public static CancelFutureOrderCommand ToCommand(this CancelFutureOrderRequest request)
        {
            return new CancelFutureOrderCommand
            (
                request.OrderId
            );
        }

        public static OpenFutureOrderCommand ToCommand(this OpenFutureOrderRequest request)
        {
            return new OpenFutureOrderCommand
            (
                request.OrderId
            );
        }
    }
}
