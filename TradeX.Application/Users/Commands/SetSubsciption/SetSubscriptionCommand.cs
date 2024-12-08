using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions.Messaging;
using TradeX.Domain.Subscriptions;

namespace TradeX.Application.Users.Commands.SetSubsciption
{
    public record SetSubscriptionCommand(Guid UserId , SubscriptionTier Tier , SubscriptionPlan Plan) : ICommand<Guid>
    {

    }
}
