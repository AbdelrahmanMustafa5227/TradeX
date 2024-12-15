using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Infrastructure.Authentication
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid JwtId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
