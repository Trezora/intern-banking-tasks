using Banking.Application.DTOs;
using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;

namespace Banking.Application.Mappings;

public static class RegisterMapper
{
    public static RegisterResponse ToRegisterResponse(this (ApplicationUserDto User, Customer Customer, IList<string> Roles) data)
    {
        var (user, customer, roles) = data;

        return new RegisterResponse
        {
            UserId = user.UserId,             
            CustomerId = customer.CustomerId,            
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.EmailAddress,
            UserName = user.UserName,
            IsEmailConfirmed = user.IsEmailConfirmed,
            Roles = roles.ToList(),
            Message = "Registration completed successfully. Please check your email to confirm your account."
        };
    }



    public static Customer ToCustomerEntity(this RegisterRequest request)
    {
        var fullName = $"{request.FirstName} {request.LastName}".Trim();

        return Customer.Create(
            new CustomerId(Guid.NewGuid()),
            new Name(fullName),
            new Email(request.Email),
            request.DateOfBirth
        );
    }
}