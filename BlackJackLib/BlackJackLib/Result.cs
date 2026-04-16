using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public string? ErrorMessage { get; protected set; }

        protected Result(bool success, string? message)
        {
            IsSuccess = success;
            ErrorMessage = message;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string message) => new Result(false, message);

    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        protected Result(bool success, T? value, string? message) : base(success, message)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null);
        new public static Result<T> Failure(string message) => new Result<T>(false, default, message);

    }
}
