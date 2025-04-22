namespace BankingApp.Domain.Shared.OperationResults;

public sealed class OperationResult<T>
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public T? Data { get; }

    private OperationResult(bool isSuccess, string message, T? data = default)
    {
        IsSuccess = isSuccess;
        Message = message; 
        Data = data;
    }

    public static OperationResult<T> Success(T? data, string message) => 
        new OperationResult<T>(true, message, data);

    public static OperationResult<T> Failure(T? data, string message) =>
        new OperationResult<T>(false, message, data);

}