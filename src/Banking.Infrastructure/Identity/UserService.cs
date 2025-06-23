using Banking.Application.DTOs;
using Banking.Application.Services;
using Banking.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Banking.Infrastructure.Identity;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> AddToRoleAsync(Guid userId, string role)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return Result.Failure(
                    "Not found.",
                    $"User with this Id{userId} not found");
            }

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(
                    "RoleAssignment Failed",
                    $"Failed to add user to role: {errors}");
            }

            return Result.Success();

        }
        catch (Exception ex)
        {
            return Result.Failure(
                "RoleAssignment Exception",
                $"An error occurred while adding user to role: {ex.Message}");
        }
    }

    public async Task<Result> CheckPasswordAsync(Guid userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return Result.Failure("Not found.", "User not found.");
        }

        var isValid = await _userManager.CheckPasswordAsync(user, password);

        if (!isValid)
        {
            return Result.Failure("Invalid.", "Invalid password.");
        }

        return Result.Success();
    }

    public async Task<Result<ApplicationUserDto>> CreateUserAsync(RegisterRequest request, string password, Guid customerId)
    {
        try
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = request.FirstName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                EmailConfirmed = false,
                CustomerId = customerId,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<ApplicationUserDto>.FailureWith(
                    "User creation failed.",
                    $"User creation failed: {errors}");
            }

            var userDto = new ApplicationUserDto
            {   
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.Email,
                DateOfBirth = user.DateOfBirth,
                UserName = user.UserName,
                IsEmailConfirmed = user.EmailConfirmed,
                CustomerId = user.CustomerId,
            };

            return Result<ApplicationUserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result<ApplicationUserDto>.FailureWith(
                "user creation failed",
                $"An error occurred while creating user: {ex.Message}");
        }
    }

    public async Task<Result<bool>> EmailExistsAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(email);
            return Result<bool>.Success(user != null);
        }
        catch (Exception ex)
        {
            return Result<bool>.FailureWith(
                "Email check failed",
                $"An error occurred while checking email existence: {ex.Message}");
        }
    }

    public async Task<Result<ApplicationUserDto>> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Result<ApplicationUserDto>.FailureWith("Email.NotFound", "User not found.");
        }

        var userDto = new ApplicationUserDto
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailAddress = email,
            DateOfBirth = user.DateOfBirth,
            UserName = user.UserName!,
            IsEmailConfirmed = user.EmailConfirmed,
            CustomerId = user.CustomerId,
        };

        return Result<ApplicationUserDto>.Success(userDto);
    }

    public async Task<Result<IList<string>>> GetUserRolesAsync(Guid id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return Result<IList<string>>.FailureWith(
                    "Not Found.",
                    "User with this id not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Result<IList<string>>.Success(roles);
        }
        catch (Exception ex)
        {
            return Result<IList<string>>.FailureWith(
                "Get role error.",
                $"An error occurred while getting user roles: {ex.Message}");
        }
    }

}