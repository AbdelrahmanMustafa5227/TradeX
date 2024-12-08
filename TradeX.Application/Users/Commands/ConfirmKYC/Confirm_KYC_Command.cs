using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Users.Commands.ConfirmKYC
{
    public record Confirm_KYC_Command(Guid UserId) : ICommand
    {
    }
}
