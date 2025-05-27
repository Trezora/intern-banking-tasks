using Banking.Application.DTOs;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;

namespace Banking.Application.Mappings;

public static class CustomerMapper
{   
    public static CustomerCreateResponse ToCustomerCreateResponse(this Customer customer)
    {
        return new CustomerCreateResponse
        {
            CustomerId = customer.CustomerId,
            FullName = customer.FullName,
            Email = customer.EmailAddress,
            DateOfBirth = customer.DateOfBirth,
        };
    }

    public static Customer ToCustomerEntity(this CreateCustomerRequest request)
    {
        return Customer.Create(
            new CustomerId(Guid.NewGuid()),            
            new Name(request.FullName),
            new Email(request.Email),
            request.DateOfBirth
        );
    }

    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.CustomerId,
            FullName = customer.FullName,
            EmailAddress = customer.EmailAddress,
            DateOfBirth = customer.DateOfBirth,
            Accounts = customer.Accounts.Select(a => new BankAccountDto
            {
                AccountNumber = a.AccountNumber,
                Balance = a.GetBalance(),
            }).ToList()
        };
    }
}