using Banking.Application.DTOs;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.Entities;
using Banking.Domain.Factories;
using Banking.Domain.ValueObjects;
using System;
using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace Banking.Application.Mappings;

public static class CustomerMapper
{   
    private static readonly CustomerFactory _customerFactory = new();

    // We want to map a Customer enitity to a CustomerResponse
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

    // We want to map a CreateCustomerRequest to a Customer entity
    public static Customer ToCustomerEntity(this CreateCustomerRequest request)
    {
        //var customerFactory = new CustomerFactory();
        var customer = _customerFactory.CreateCustomer(
            new Name(request.FullName),
            new Email(request.Email),
            request.DateOfBirth
        );

        return customer;
    }

    // We want to map a customer entity to a customerDto 
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