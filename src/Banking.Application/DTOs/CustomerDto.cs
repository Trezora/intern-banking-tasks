namespace Banking.Application.DTOs;

public sealed class CustomerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public List<BankAccountDto> Accounts { get; set; } = new();

}