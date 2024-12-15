using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeX.Application.Exceptions;

namespace TradeX.Application.Behaviors
{
    internal class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validator)
        {
            _validators = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var validationErrors = _validators
                    .Select(x => x.Validate(request))
                    .Where(x => x.Errors.Any())
                    .SelectMany(x => x.Errors)
                    .Select(x => new ValidationError(x.PropertyName , x.ErrorMessage));

            if(validationErrors.Any())
            {
                throw new Exceptions.ValidationException(validationErrors);
            }

            return await next();
        }
    }
}
