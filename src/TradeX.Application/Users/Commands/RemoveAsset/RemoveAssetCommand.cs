using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Users.Commands.AddAsset
{
    internal record RemoveAssetCommand(Guid UserId , Guid CryptoId , decimal Amount) : ICommand
    {
    }
}
