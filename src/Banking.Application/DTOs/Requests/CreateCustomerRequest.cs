using System.ComponentModel.DataAnnotations;

namespace Banking.Application.DTOs.Requests;

public sealed record CreateCustomerRequest
{   
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }

}