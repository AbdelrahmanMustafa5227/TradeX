using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Infrastructure.Authentication
{
    internal class AuthenticationOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AuthenticationOptions _options;

        public AuthenticationOptionsSetup(IOptions<AuthenticationOptions> options)
        {
            _options = options.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.Audience = _options.Audience;

            options.TokenValidationParameters.ValidIssuer = _options.Issuer;
            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            options.TokenValidationParameters.ValidateAudience = false;
            options.TokenValidationParameters.ValidateIssuer = false;
            options.TokenValidationParameters.ValidateIssuerSigningKey = true;
            options.TokenValidationParameters.ValidateLifetime = true;
            options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }
    }
}
