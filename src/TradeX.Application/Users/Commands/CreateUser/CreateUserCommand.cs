using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(string firstName, string lastName, string email, string password,
        PaymentMethod paymentMethod) : ICommand<User>
    {
    }
}
