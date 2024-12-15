using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Authentication.Revoke
{
    public record RevokeCommand (Guid UserId) : ICommand
    {
    }
}
