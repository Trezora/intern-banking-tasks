namespace Shared.OperationResults;

public sealed class OperationResult
{
    public bool Success { get; }
    public string Message { get; }
    
    private OperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static OperationResult Succeeded(string message) 
        => new(true, message);

    public static OperationResult Failed(string message)
        => new(false, message);
}