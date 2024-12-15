using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Exceptions
{
    public record ValidationError (string PropertyName , string ErrorMessage)
    {
    }
}
