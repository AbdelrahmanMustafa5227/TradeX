using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Application.Abstractions.Authentication;
using TradeX.Domain.Abstractions;

namespace TradeX.Application.Authentication.Revoke
{
    internal class RevokeCommandHandler : ICommandHandler<RevokeCommand>
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUserContext _userContext;

        public RevokeCommandHandler(IJwtTokenProvider jwtTokenProvider, IUserContext userContext)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _userContext = userContext;
        }

        public async Task<Result> Handle(RevokeCommand request, CancellationToken cancellationToken)
        {
            if (_userContext.UserId != request.UserId)
                return Result.Failure(new Error("Auth" , "You aren't allowed to do this operation"));

            await _jwtTokenProvider.Revoke(request.UserId);
            return Result.Success();
        }
    }
}
