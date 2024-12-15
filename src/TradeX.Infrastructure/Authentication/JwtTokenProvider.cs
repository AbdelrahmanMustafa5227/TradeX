using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions;
using TradeX.Application.Abstractions.Authentication;
using TradeX.Domain.Abstractions;
using TradeX.Domain.Users;
using TradeX.Infrastructure.Persistance;

namespace TradeX.Infrastructure.Authentication
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly AuthenticationOptions _options;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtTokenProvider(IOptions<AuthenticationOptions> options, ApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
        {
            _options = options.Value;
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public JwtToken GenerateAccessTokenFromLogin(User user)
        {
            var jti = Guid.NewGuid();
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti , jti.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Audience = _options.Audience,
                Issuer = _options.Issuer,
                Expires = _dateTimeProvider.UtcNow.AddMinutes(_options.AccessExpiresInMinutes)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = PersistRefreshToken(user.Id, jti);

            return new JwtToken(tokenHandler.WriteToken(token), refreshToken);
        }

        private Guid PersistRefreshToken(Guid UserId, Guid tokenId)
        {
            RefreshToken refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                UserId = UserId,
                JwtId = tokenId,
                ExpiresAt = _dateTimeProvider.UtcNow.AddMinutes(_options.RefreshExpiresInMinutes)
            };

            _dbContext.Set<RefreshToken>().Add(refreshToken);
            _dbContext.SaveChanges();

            return refreshToken.Id;
        }

        public async Task<Result<JwtToken>> RefreshAccessToken(JwtToken token)
        {
            // Get refresh token from DB
            var TokenFromDb = await _dbContext.Set<RefreshToken>().FirstOrDefaultAsync(u => u.Id == token.RefreshToken);
            if (TokenFromDb == null)
                return Result.Failure<JwtToken>(new Error("","Invalid Refresh Token"));

            // Check if the refresh token is expired 
            if (TokenFromDb.ExpiresAt < _dateTimeProvider.UtcNow)
                return Result.Failure<JwtToken>(new Error("", "Expired Refresh Token"));

            // Check if refresh token is related to JWT token
            var accessTokenInfo = new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);
            var userId = accessTokenInfo.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)!.Value;
            var jti = accessTokenInfo.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
            if (userId != TokenFromDb.UserId.ToString() || jti != TokenFromDb.JwtId.ToString())
                return Result.Failure<JwtToken>(new Error("", "Invalid Refresh Token"));

            // Check if the token belongs to a user
            var user = await _dbContext.Set<User>().FirstOrDefaultAsync(x => x.Id == TokenFromDb.UserId);
            if (user == null)
                return Result.Failure<JwtToken>(new Error("", "Invalid Refresh Token"));

            //Generate new Access Tokens
            var newJti = Guid.NewGuid();
            var accessToken = GenerateAccessTokenFromRefreshToken(user, newJti);

            //Update new JTI
            TokenFromDb.JwtId = newJti;
            _dbContext.SaveChanges();

            return Result.Success(new JwtToken(accessToken, TokenFromDb.Id));
        }

        private string GenerateAccessTokenFromRefreshToken(User user, Guid jti)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti , jti.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Audience = _options.Audience,
                Issuer = _options.Issuer,
                Expires = _dateTimeProvider.UtcNow.AddMinutes(_options.AccessExpiresInMinutes)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task Revoke (Guid UserId)
        {
            await _dbContext.Set<RefreshToken>().Where(x => x.UserId == UserId).ExecuteDeleteAsync();
        }


    }
}
