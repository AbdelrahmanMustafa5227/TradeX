using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Users;

namespace TradeX.Application.Users.Commands.CreateUser
{
    internal class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.email).EmailAddress();
            RuleFor(x => x.paymentMethod).IsInEnum();
        }
    }
}
