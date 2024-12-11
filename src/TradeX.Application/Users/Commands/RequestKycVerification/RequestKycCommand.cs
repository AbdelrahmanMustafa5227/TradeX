using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Abstractions.Messaging;
using TradeX.Domain.Abstractions;

namespace TradeX.Application.Users.Commands.RequestKycVerification
{
    internal record RequestKycCommand(Guid userId) : ICommand
    {

    }
}
