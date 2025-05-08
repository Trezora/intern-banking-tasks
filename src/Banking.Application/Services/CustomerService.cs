using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Application.Mappings;
using Banking.Domain.Entities;
using Banking.Domain.Exceptions;
using Banking.Domain.Repositories;
using MediatR;

namespace Banking.Application.Services;

public class CustomerService : ICustomerService
{   
    private readonly ICustomerRespository _customerRepository;
    private readonly IMediator _mediator;

    public CustomerService(ICustomerRespository customerRespository, IMediator mediator)
    {
        _customerRepository = customerRespository;
        _mediator = mediator;
    }

    public async Task<ApiResponse> CreateCustomerAsync(CreateCustomerRequest request)
    {   
        try {
            var customer = request.ToCustomerEntity();

            await _customerRepository.AddAsync(customer);   

            foreach (var domainEvent in customer.DomainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            customer.ClearDomainEvents();

            var response = customer.ToResponse();

            return new ApiResponse(true, "Customer created successfully", response);
            
        } catch (EmailAlreadyExistException)
        {
            return new ApiResponse(false, "Customer with this email already exists", null);
        }
    }

    public async Task<ApiResponse> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            return new ApiResponse(false, $"Customer with ID {id} not found", null);
        }

        var CustomerDto = customer.ToDto(); 

        return new ApiResponse(true, "Customer retrieved successfully", CustomerDto);
    }

    public async Task<ApiResponse> GetAllCustomerAsync()
    {
        var customers = await _customerRepository.GetAllCustomerAsync();
    
        if (customers == null || !customers.Any())
        {
          return new ApiResponse(false, "No customers found", null);
        }
    
        var customersDto = customers.Select(c => c.ToDto()).ToList();
        return new ApiResponse(true, "Customers retrieved successfully", customersDto);
    }
}