using System.ComponentModel.DataAnnotations;

namespace Banking.Application.DTOs.Requests;

public sealed record CreateBankAccountRequest
{
    public decimal Balance { get; set; }

    [Required]
    public Guid CustomerId { get; set; }
}