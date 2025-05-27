using System.Threading.Tasks;
using Banking.Application.Commands;
using Banking.Application.DTOs.Requests;
using Banking.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers;

[Route("api/[controller]")]
public sealed class CustomerController(ISender sender) : ApiController(sender)
{

    [HttpGet("id")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var query = new GetCustomerByIdQuery(id);

        var result = await Sender.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var query = new GetAllCustomersQuery();

        var result = await Sender.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        var command = new CreateCustomerCommand(request);

        var result = await Sender.Send(command);

        return result.IsSuccess
            ? CreatedAtAction(nameof(CreateCustomer), new { customerId = result.Value.CustomerId }, result.Value)
            : HandleFailure(result);
    }
   
}