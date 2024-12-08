using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Subscriptions.Commands.AddAlert
{
    public record AddAlertCommand(Guid UserId , Guid CryptoId, decimal Price) : ICommand
    {
    }
}
