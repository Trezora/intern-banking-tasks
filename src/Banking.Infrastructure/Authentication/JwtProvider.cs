using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Banking.Application.Abstractions;
using Banking.Application.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Banking.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public  (string AccessToken, string RefreshToken, DateTime ExpiresAt) GenerateToken(ApplicationUserDto userDto)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userDto.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userDto.EmailAddress)
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secretkey)),
            SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddHours(1);

        var accessToken = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            expiresAt,
            signingCredentials);

        string accessTokenValue = new JwtSecurityTokenHandler().WriteToken(accessToken);

        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        return (accessTokenValue, refreshToken, expiresAt);
    }
}