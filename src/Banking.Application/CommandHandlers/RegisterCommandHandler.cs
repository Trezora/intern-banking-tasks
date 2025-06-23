using Banking.Application.Commands;
using Banking.Application.DTOs;
using Banking.Application.Mappings;
using Banking.Application.Services;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;
using MediatR;

namespace Banking.Application.CommandHandlers;

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUserService _userService;
    private readonly ICustomerRespository _customerRespository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
            IUserService userService,
            IUnitOfWork unitOfWork,
            ICustomerRespository customerRespository)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _customerRespository = customerRespository;
    }

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailExistsResult = await _userService.EmailExistsAsync(request.Request.Email);

            if (!emailExistsResult.IsSuccess)
            {
                return Result<RegisterResponse>.FailureWith(
                    emailExistsResult.Error.Code,
                    emailExistsResult.Error.Description!);
            }

            if (emailExistsResult.Value)
            {
                return Result<RegisterResponse>.FailureWith(
                    "email error.",
                    $"Email: {request.Request.Email} already exists.");
            }

            var customer = request.Request.ToCustomerEntity();

            await _customerRespository.AddAsync(customer);

            var createUserResult = await _userService.CreateUserAsync(
                request.Request,
                request.Request.Password,
                customer.CustomerId.Value);

            if (!createUserResult.IsSuccess)
            {
                return Result<RegisterResponse>.FailureWith(
                    createUserResult.Error.Code,
                    createUserResult.Error.Description!);
            }

            var applicationUserDto = createUserResult.Value;

            var userId = applicationUserDto.UserId;

            var addToRoleResult = await _userService.AddToRoleAsync(userId, "customer");

            if (!addToRoleResult.IsSuccess)
            {
                return Result<RegisterResponse>.FailureWith(
                    addToRoleResult.Error.Code,
                    addToRoleResult.Error.Description!);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userRolesResult = await _userService.GetUserRolesAsync(userId);

            if (!userRolesResult.IsSuccess)
            {
                return Result<RegisterResponse>.FailureWith(
                    userRolesResult.Error.Code,
                    userRolesResult.Error.Description!);
            }

            var data = (applicationUserDto, customer, userRolesResult.Value);
            var registerResponse = data.ToRegisterResponse();

            return Result<RegisterResponse>.Success(registerResponse);
        }
        catch (Exception ex)
        {
            return Result<RegisterResponse>.FailureWith(
                "Registration error.",
                $"Registration failed: {ex.Message}");
        }
    }
    
}