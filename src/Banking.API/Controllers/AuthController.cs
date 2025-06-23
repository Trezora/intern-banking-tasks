using Banking.Application.Commands;
using Banking.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers;

[Route("api/[controller]")]
public sealed class AuthContoller(ISender sender) : ApiController(sender)
{

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

   
}