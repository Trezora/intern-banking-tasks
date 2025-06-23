using Banking.Application.DTOs;
using Banking.Domain.Shared;

namespace Banking.Application.Services;

public interface IUserService
{
    Task<Result<bool>> EmailExistsAsync(string email);
    Task<Result<ApplicationUserDto>> CreateUserAsync(RegisterRequest request, string password, Guid customerId);
    Task<Result<ApplicationUserDto>> GetUserByEmailAsync(string email);
    Task<Result<IList<string>>> GetUserRolesAsync(Guid id);
    Task<Result> AddToRoleAsync(Guid userId, string role);
    Task<Result> CheckPasswordAsync(Guid userId, string password);

}