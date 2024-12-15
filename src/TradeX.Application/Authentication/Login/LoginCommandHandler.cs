using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions.Authentication;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.Login
{
    internal class LoginCommandHandler : ICommandHandler<LoginCommand, JwtToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public LoginCommandHandler(IUserRepository userRepository, IJwtTokenProvider jwtTokenProvider)
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<Result<JwtToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || user.Password != request.Password)
                return Result.Failure<JwtToken>(UserErrors.InvalidCredetials);

            var jwtToken = _jwtTokenProvider.GenerateAccessTokenFromLogin(user);

            return Result.Success(jwtToken);
        }
    }
}
