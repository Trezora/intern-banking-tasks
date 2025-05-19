using System.Threading.Tasks;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof (ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof (ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse(false, "Invalid request data", ModelState));
        }

        var result = await _customerService.CreateCustomerAsync(request);

        if (result.Success)
        {
            
            var customerResponse = (CustomerCreateResponse)result.Data!;
            return CreatedAtAction(nameof(GetCustomerById), new { id = customerResponse.CustomerId }, result);
        }

        return BadRequest(result);
    }

    [HttpPost("open_account")]
    [ProducesResponseType(typeof (ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof (ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> OpenNewAccount([FromBody] CreateBankAccountRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse(false, "Invalid request data", ModelState));
        }      

        var result  = await _customerService.OpenNewBankAccountAsync(request);

        if (result.Success)
        {
            var bankAccountResponse = (BankAccountCreateResponse)result.Data!;
            return CreatedAtRoute("GetBankAccountById", new {id  = bankAccountResponse.AccountNumber}, result);
        }

        return BadRequest(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var response = await _customerService.GetCustomerByIdAsync(id);

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCustomer()
    {
        var response = await _customerService.GetAllCustomerAsync();

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}