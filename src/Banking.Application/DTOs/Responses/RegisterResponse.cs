namespace Banking.Application.DTOs;

public sealed class RegisterResponse
{
    public Guid UserId { get; set; }          
    public Guid CustomerId { get; set; }      
    public string FirstName { get; set; } = string.Empty; 
    public string LastName { get; set; } = string.Empty;  
    public string Email { get; set; } = string.Empty;    
    public string UserName { get; set; } = string.Empty; 
    public bool IsEmailConfirmed { get; set; }          
    public List<string> Roles { get; set; } = new();   
    public string Message { get; set; } = string.Empty; 
}
