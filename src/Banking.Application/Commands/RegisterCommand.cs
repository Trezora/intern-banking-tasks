using Banking.Application.DTOs;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.Commands;

public class RegisterCommand : IRequest<Result<RegisterResponse>>
{
    public RegisterRequest Request { get; }
    public RegisterCommand(RegisterRequest registerRequest)
    {
        Request = registerRequest;
    }
}