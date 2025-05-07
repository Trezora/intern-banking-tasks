using System.ComponentModel.DataAnnotations;

namespace Banking.Application.DTOs.Requests;

public sealed record CreateCustomerRequest
{   
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    [StringLength(20, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

}