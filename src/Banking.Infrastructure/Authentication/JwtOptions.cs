namespace Banking.Infrastructure.Authentication;

public class JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Secretkey { get; init; }
}