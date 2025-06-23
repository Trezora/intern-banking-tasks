using Banking.Application.DTOs;

namespace Banking.Application.Abstractions;

public interface IJwtProvider
{
     (string AccessToken, string RefreshToken, DateTime ExpiresAt) GenerateToken(ApplicationUserDto userDto);
}