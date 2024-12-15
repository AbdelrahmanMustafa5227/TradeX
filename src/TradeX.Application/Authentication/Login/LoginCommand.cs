using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions.Authentication;

namespace TradeX.Application.Users.Commands.Login
{
    public record LoginCommand(string Email , string Password) : ICommand<JwtToken>
    {

    }
}
