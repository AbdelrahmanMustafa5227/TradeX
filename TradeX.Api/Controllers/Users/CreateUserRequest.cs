using TradeX.Domain.Users;

namespace TradeX.Api.Controllers.Users
{
    public record CreateUserRequest(string firstName, string lastName, string email, string password,
        PaymentMethod paymentMethod)
    {
    }
}
