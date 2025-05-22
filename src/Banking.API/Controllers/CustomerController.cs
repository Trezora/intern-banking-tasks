using System.Threading.Tasks;
using Banking.Application.Commands;
using Banking.Application.DTOs.Requests;
using Banking.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;
    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var query = new GetCustomerByIdQuery(id);

        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var query = new GetAllCustomersQuery();

        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        if (!ModelState.IsValid) return BadRequest();

        var command = new CreateCustomerCommand(request);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? CreatedAtAction(nameof(CreateCustomer), new { customerId = result.Value.CustomerId }, result.Value)
            : BadRequest(result.Error);
    }
   
}