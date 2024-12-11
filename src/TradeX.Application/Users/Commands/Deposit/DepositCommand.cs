using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Users.Commands.Deposit
{
    public record DepositCommand(Guid UserId , decimal DepositAmount) : ICommand
    {
    }
}
