using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackLib
{
    public class Result
    {
        /// <summary>
        /// True if Result was successfull
        /// </summary>
        public bool IsSuccess { get; protected set; }
        /// <summary>
        /// True if Result was not successfull
        /// </summary>
        public bool IsFailure => !IsSuccess;
        /// <summary>
        /// In case Result is not successfull this attribute will provide a message with what went wrong
        /// </summary>
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
        /// <summary>
        /// Will contain value in case of success
        /// </summary>
        public T? Value { get; }

        protected Result(bool success, T? value, string? message) : base(success, message)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, value, null);
        new public static Result<T> Failure(string message) => new Result<T>(false, default, message);

    }
}
