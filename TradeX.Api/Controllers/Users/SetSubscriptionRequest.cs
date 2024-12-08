using TradeX.Domain.Subscriptions;

namespace TradeX.Api.Controllers.Users
{
    public record SetSubscriptionRequest(Guid UserId, SubscriptionTier Tier, SubscriptionPlan Plan)
    {
    }
}
