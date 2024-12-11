using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Subscriptions.Commands.RemoveAlert
{
    public record RemoveAlertCommand(Guid UserId ,Guid AlertId) : ICommand
    {
    }
}
