using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Infrastructure.Authentication
{
    public class AuthenticationOptions
    {
        public const string SectionName = "AuthenticationOptions";

        public string SecretKey { get; init; } = String.Empty;
        public string Issuer { get; init; } = String.Empty;
        public string Audience { get; init; } = String.Empty;
        public int AccessExpiresInMinutes { get; init; }
        public int RefreshExpiresInMinutes { get; init; }
    }
}
