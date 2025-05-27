namespace Banking.Domain.Shared;

public sealed class ValidationResult : Result, IValidationResult
{
    public Error[] Errors { get; }
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

public sealed class ValidationResult<T> : Result<T>, IValidationResult
{   
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors)
        : base(false, default, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public static ValidationResult<T> WithErrors(Error[] errors) => new(errors);
}
