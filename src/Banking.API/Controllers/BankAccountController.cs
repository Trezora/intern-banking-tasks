using Banking.Application.Commands;
using Banking.Application.DTOs.Requests;
using Banking.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("account_number")]
    public async Task<IActionResult> GetBankAccountByAccountNumber(Guid accountNumber)
    {
        var query = new GetBankAccountByAccountNumberQuery(accountNumber);

        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpGet("customer_id")]
    public async Task<IActionResult> GetBankAccountsByCustomerId(Guid customerId)
    {
        var query = new GetBankAccountsByCustomerIdQuery(customerId);

        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountRequest request)
    {
        if (!ModelState.IsValid) return BadRequest();

        var command = new CreateBankAccountCommand(request);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? CreatedAtAction(nameof(CreateBankAccount), new { accountNumber = result.Value.AccountNumber }, result.Value)
            : BadRequest(result.Error);
    }
}