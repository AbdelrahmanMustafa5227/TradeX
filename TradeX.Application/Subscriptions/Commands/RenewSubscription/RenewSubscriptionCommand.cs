using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Subscriptions.Commands.RenewSubscription
{
    public record RenewSubscriptionCommand(Guid UserId) : ICommand
    {
    }
}
