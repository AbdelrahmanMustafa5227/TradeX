using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Application.Abstractions.Messaging
{
    internal interface ICommandHandler<TCommand,TResponse> : IRequestHandler<TCommand, Result<TResponse>> 
        where TCommand : ICommand<TResponse>
    {
    }

    internal interface ICommandHandler<TCommand> : IRequestHandler<TCommand,Result>
        where TCommand : ICommand
    {
    }

}
