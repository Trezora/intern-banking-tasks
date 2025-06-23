namespace Banking.Application.DTOs;

public sealed class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public Guid UserId { get; set; }  
    public Guid CustomerId { get; set; }
    public DateTime ExpiresAt;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();   
}