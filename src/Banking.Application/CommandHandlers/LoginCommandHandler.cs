using Banking.Application.Abstractions;
using Banking.Application.Commands;
using Banking.Application.DTOs;
using Banking.Application.Mappings;
using Banking.Application.Services;
using Banking.Domain.Shared;
using MediatR;
namespace Banking.Application.CommandHandlers;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{   
    private readonly IUserService _userService;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        IUserService userService,
        IJwtProvider jwtProvider)
    {
        _userService = userService;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _userService.GetUserByEmailAsync(request.Request.Email);

        if (!userResult.IsSuccess)
        {
            return Result<LoginResponse>.FailureWith(
                userResult.Error.Code,
                userResult.Error.Description!);
        }

        var isPasswordValidResult = await _userService.CheckPasswordAsync(
            userResult.Value.UserId,
            request.Request.Password);

        if (!isPasswordValidResult.IsSuccess)
        {
            return Result<LoginResponse>.FailureWith(
                isPasswordValidResult.Error.Code,
                isPasswordValidResult.Error.Description!);
        }

        var (accessToken, refreshToken, expiresAt) = _jwtProvider.GenerateToken(userResult.Value);

        var userRolesResult = await _userService.GetUserRolesAsync(userResult.Value.UserId);

        if (!userRolesResult.IsSuccess)
        {
            return Result<LoginResponse>.FailureWith(
                userRolesResult.Error.Code,
                userRolesResult.Error.Description!);
        }

        var loginResponse = userResult.Value.ToLoginResponse(
            accessToken,
            refreshToken,
            expiresAt,
            userRolesResult.Value.ToList());

        return Result<LoginResponse>.Success(loginResponse);    
    }
}