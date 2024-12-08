using TradeX.Api.Controllers.Subscriptions;
using TradeX.Application.Subscriptions.Commands.AddAlert;
using TradeX.Application.Subscriptions.Commands.RemoveAlert;
using TradeX.Application.Subscriptions.Commands.RenewSubscription;

namespace TradeX.Api.Mapping
{
    public static class SubscriptionMapping
    {
        public static AddAlertCommand ToCommand(this AddAlertRequest request)
        {
            return new AddAlertCommand
            (
                request.UserId,
                request.CryptoId,
                request.Price
            );
        }

        public static RemoveAlertCommand ToCommand(this RemoveAlertRequest request)
        {
            return new RemoveAlertCommand
            (
                request.UserId,
                request.AlertId
            );
        }

        public static RenewSubscriptionCommand ToCommand(this RenewSubscriptionRequest request)
        {
            return new RenewSubscriptionCommand
            (
                request.UserId
            );
        }
    }
}
