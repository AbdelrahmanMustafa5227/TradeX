using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Cryptos.Queries.GetAll
{
    public record GetAllCryptosResponse (string Name , string Symbol , decimal Price)
    {
    }
}
