using Banking.Application.DTOs;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.Commands;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public LoginRequest Request { get; }
    public LoginCommand(LoginRequest loginRequest)
    {
        Request = loginRequest;
    }
}