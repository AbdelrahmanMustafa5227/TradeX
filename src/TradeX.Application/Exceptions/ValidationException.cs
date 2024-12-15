using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<ValidationError> Errors;

        public ValidationException(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }
    }
}
