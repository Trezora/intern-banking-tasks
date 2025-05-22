using Banking.Application.DTOs;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Application.Mappings;
using Banking.Domain.Entities;
using Banking.Domain.Exceptions;
using Banking.Domain.Repositories;
using Banking.Domain.ValueObjects;
using Banking.Shared.OperationResults;
using MediatR;

namespace Banking.Application.Services;

public class CustomerService : ICustomerService
{   
    private readonly ICustomerRespository _customerRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public CustomerService(ICustomerRespository customerRespository, 
                           IBankAccountRepository bankAccountRepository)
    {
        _customerRepository = customerRespository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Result<CustomerDto>> TryGetCustomerByIdAsync(Guid id)
    {
        var customer = await _customerRepository.GetByCustomerIdAsync(id);

        if (customer == null)
            return Result<CustomerDto>.FailureWith($"Customer with ID {id} was not found.");
    

        return Result<CustomerDto>.Success(customer.ToDto());
    }


    public async Task<Result<IEnumerable<CustomerDto>>> TryGetAllCustomerAsync()
    {
        var customers = await _customerRepository.GetAllCustomerAsync();

        if (customers == null || !customers.Any())
            return Result<IEnumerable<CustomerDto>>.FailureWith("No customers found.");
        

        var customersDto = customers.Select(c => c.ToDto());
        
        return Result<IEnumerable<CustomerDto>>.Success(customersDto);
    }
}