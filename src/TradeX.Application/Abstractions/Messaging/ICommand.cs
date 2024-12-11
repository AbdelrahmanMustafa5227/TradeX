using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Domain.Abstractions;

namespace TradeX.Application.Abstractions.Messaging
{
    internal interface ICommand : IRequest<Result> , IBaseCommand
    {

    }

    internal interface ICommand<TResponse> : IRequest<Result<TResponse>> , IBaseCommand
    {

    }

    internal interface IBaseCommand;
}
