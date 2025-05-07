using Banking.Application.DTOs;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.Entities;
using Banking.Domain.Factories;
using Banking.Domain.ValueObjects;
using System;
using System.Linq;

namespace Banking.Application.Mappings;

public static class CustomerMapper
{   
    // We want to map a Customer enitity to a CustomerResponse
    public static CustomerResponse ToResponse(this Customer customer)
    {
        return new CustomerResponse
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
        var customerFactory = new CustomerFactory();
        var customer = customerFactory.CreateCustomer(
            request.FullName, 
            request.Email,
            request.DateOfBirth
        );

        return customer;
    }
}