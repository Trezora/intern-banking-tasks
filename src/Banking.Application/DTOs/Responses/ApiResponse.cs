namespace Banking.Application.DTOs.Responses;

public sealed class ApiResponses
{
    public bool Success { get; set; }
    public string Message { get; set; } 
    public object? Data { get; set; }

    public ApiResponses(bool success, string message, object? data)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}
