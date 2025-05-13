using Banking.Application.DTOs.Responses;
using Banking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    [HttpGet("{id}", Name = "GetBankAccountById")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBankAccountByAccountNumber(Guid id)
    {
        var response = await _bankAccountService.GetBankAccountsByCustomerIdAsync(id);

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}