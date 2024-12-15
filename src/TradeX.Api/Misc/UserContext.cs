using System.Security.Claims;
using TradeX.Application.Abstractions;

namespace TradeX.Api.Misc
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid? UserId => Guid.TryParse(
            _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier) ,
            out var parsed ) ? parsed : null;
    }
}
