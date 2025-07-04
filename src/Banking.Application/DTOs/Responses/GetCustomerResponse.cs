namespace Banking.Application.DTOs.Responses;

public sealed record GetCustomerResponse
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public List<BankAccountDto> Accounts { get; set; } = new();
}