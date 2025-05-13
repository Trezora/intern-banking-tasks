namespace Banking.Application.DTOs.Responses;

public sealed record BankAccountCreateResponse
{
    public Guid AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public Guid CustomerId { get; set; }
}