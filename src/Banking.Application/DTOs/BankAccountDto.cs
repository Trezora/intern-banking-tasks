namespace Banking.Application.DTOs;

public sealed class BankAccountDto
{
    public Guid AccountNumber { get; set; }
    public decimal Balance { get; set; }
}