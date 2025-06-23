using Banking.Domain.ValueObjects;

namespace Banking.Application.DTOs;

public sealed class ApplicationUserDto
{   
    public Guid UserId { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string UserName { get; init; } = string.Empty;
    public bool IsEmailConfirmed { get; init; }
    public Guid CustomerId { get; set; }
}