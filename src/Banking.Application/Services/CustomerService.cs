using Banking.Application.DTOs;
using Banking.Application.Mappings;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;

namespace Banking.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRespository _customerRepository;

    public CustomerService(ICustomerRespository customerRespository)
    {
        _customerRepository = customerRespository;
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