using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeX.Domain.Abstractions
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; private set; }

        protected Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Failure(Error error) => new Result(false, error);
        public static Result Success() => new Result(true, Error.None);

        public static Result<T> Failure<T>(Error error) => new Result<T>(false,error, default);
        public static Result<T> Success<T>(T value) => new Result<T>(true, Error.None , value);

        public static Result<T> Create<T>(T value) => value is not null ? Success<T>(value) : Failure<T>(Error.Null);

    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        public Result(bool isSuccess , Error? error , T? value) : base(isSuccess , error)
        {
            _value = value;
        }

        [NotNull]
        public T Value => IsSuccess ? _value! : throw new InvalidOperationException()!;

        public static implicit operator Result<T>(T value) => Create<T>(value);
        
    }
}
