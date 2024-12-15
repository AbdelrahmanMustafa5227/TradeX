using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions.Authentication;
using TradeX.Domain.Abstractions;

namespace TradeX.Application.Authentication.Refresh
{
    internal class RefreshCommandHandler : ICommandHandler<RefreshCommand, JwtToken>
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public RefreshCommandHandler(IJwtTokenProvider jwtTokenProvider)
        {
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<Result<JwtToken>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var result = await _jwtTokenProvider.RefreshAccessToken(new JwtToken(request.AccessToken, request.RefreshToken));

            return result;
        }
    }
}
