namespace Shared.OperationResults;

public sealed class OperationResult
{
    public bool Result { get; }
    public string Message { get; }
    
    private OperationResult(bool result, string message)
    {
        Result = result;
        Message = message;
    }

    public static OperationResult Succeeded(string message) 
        => new(true, message);

    public static OperationResult Failed(string message)
        => new(false, message);
}