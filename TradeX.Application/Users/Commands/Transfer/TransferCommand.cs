using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Users.Commands.Transfer
{
    public record TransferCommand(Guid SenderId , Guid RecepientId , decimal Amount) : ICommand
    {
    }
}
