using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Application.Abstractions.Authentication;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.Login
{
    internal class LoginCommandHandler : ICommandHandler<LoginCommand, JwtToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IUserRepository userRepository, IJwtTokenProvider jwtTokenProvider, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<JwtToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if(user == null)
                return Result.Failure<JwtToken>(UserErrors.InvalidCredetials);

            bool isCorrectPass = _passwordHasher.Verify(request.Password, user.Password);
            if (!isCorrectPass)
                return Result.Failure<JwtToken>(UserErrors.InvalidCredetials);

            var jwtToken = _jwtTokenProvider.GenerateAccessTokenFromLogin(user);

            return Result.Success(jwtToken);
        }
    }
}
