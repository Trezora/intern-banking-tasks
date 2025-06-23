using Banking.Application.DTOs;

namespace Banking.Application.Mappings;

public static class LoginMapper
{
    public static LoginResponse ToLoginResponse(
        this ApplicationUserDto userDto,
        string accessToken,
        string refreshToken,
        DateTime expiresAt,
        List<string> roles)
    {
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            UserId = userDto.UserId,
            CustomerId = userDto.CustomerId,
            Email = userDto.EmailAddress,
            Roles = roles
        };
    }

}