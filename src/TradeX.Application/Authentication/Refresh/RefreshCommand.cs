using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions.Authentication;

namespace TradeX.Application.Authentication.Refresh
{
    public record RefreshCommand(string AccessToken , Guid RefreshToken) : ICommand<JwtToken>
    {
    }
}
