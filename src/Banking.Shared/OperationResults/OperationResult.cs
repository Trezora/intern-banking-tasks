using System.Diagnostics.CodeAnalysis;

namespace Banking.Shared.OperationResults;

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
        => new(true, string.Empty);
    public static Result Failure(string error)
        => new(false, error);
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    // This attribute tells the compiler that Value is not null when IsSuccess is true
    [MemberNotNullWhen(true, nameof(Value))]
    public new bool IsSuccess => base.IsSuccess;

    private Result(bool isSuccess, T? value, string error) 
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
        => new(true, value, string.Empty);
    
    public static Result<T> FailureWith(string error)
        => new(false, default, error);
}