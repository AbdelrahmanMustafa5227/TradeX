using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Application.Abstractions.Messaging
{
    internal interface IQuery<T> : IRequest<Result<T>>
    {

    }
}
