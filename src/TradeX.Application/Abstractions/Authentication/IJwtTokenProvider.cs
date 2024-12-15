using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Application.Abstractions.Authentication
{
    public interface IJwtTokenProvider
    {
        JwtToken GenerateAccessTokenFromLogin(User user);
        Task<Result<JwtToken>> RefreshAccessToken(JwtToken token);
        Task Revoke(Guid UserId);
    }
}
