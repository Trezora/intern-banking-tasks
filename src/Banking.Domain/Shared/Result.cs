using System.Diagnostics.CodeAnalysis;

namespace Banking.Domain.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
        => new(true, new Error(""));
    public static Result Failure(string error)
        => new(false, new Error(error));
}

public class Result<T> : Result
{
    public T? Value { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    public new bool IsSuccess => base.IsSuccess;

    protected Result(bool isSuccess, T? value, Error error) 
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
        => new(true, value, new Error(""));
    
    public static Result<T> FailureWith(string error)
        => new(false, default, new Error(error));
}