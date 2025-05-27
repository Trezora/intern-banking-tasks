using System.ComponentModel.DataAnnotations;

namespace Banking.Application.DTOs.Requests;

public sealed record CreateBankAccountRequest
{
    public decimal InitialDeposit { get; set; }
    public Guid CustomerId { get; set; }
}