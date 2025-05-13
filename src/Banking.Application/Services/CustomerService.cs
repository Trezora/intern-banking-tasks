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
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IMediator _mediator;

    public CustomerService(ICustomerRespository customerRespository, 
                           IBankAccountRepository bankAccountRepository, 
                           IMediator mediator)
    {
        _customerRepository = customerRespository;
        _bankAccountRepository = bankAccountRepository;
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
            
        } 
        catch (Exception ex) when (
            ex is EmailAlreadyExistException ||
            ex is EmptyCustomerEmailException ||
            ex is InvalidEmailFormatException ||
            ex is EmptyCustomerNameException)
        {
            return new ApiResponse(false, ex.Message, null);
        }
    }

    public async Task<ApiResponse> OpenNewBankAccountAsync(CreateBankAccountRequest request)
    {
        try
        {
            var bankAccount = request.ToBankAccountEntity();
            var customerId = bankAccount.CustomerId;
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return new ApiResponse(false, $"Customer with ID {customerId} not found.", null);
            }  

            customer.OpenNewAccount(bankAccount.GetBalance());
            await _bankAccountRepository.AddAsync(bankAccount);

            var response = bankAccount.ToResponse();
            return new ApiResponse(true, "Bank Account created successfully", response);
        }
        catch
        {
            return new ApiResponse(false, "An error occurred while creating the bank account.", null);
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