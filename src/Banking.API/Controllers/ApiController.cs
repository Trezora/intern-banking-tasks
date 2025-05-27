using Banking.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers;

[ApiController]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;

    protected IActionResult HandleFailure(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error", StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Description,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };

 
}