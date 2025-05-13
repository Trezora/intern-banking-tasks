using Banking.Application.DTOs.Responses;
using Banking.Application.Mappings;
using Banking.Domain.Entities;
using Banking.Domain.Repositories;
using MediatR;

namespace Banking.Application.Services;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICustomerRespository _customerRepository;
    private readonly IMediator _mediator;

    public BankAccountService(IBankAccountRepository bankAccountRepository,
                              ICustomerRespository customerRespository, 
                              IMediator mediator)
    {
        _bankAccountRepository = bankAccountRepository;
        _customerRepository = customerRespository;
        _mediator = mediator;
    }
    public async Task<ApiResponse> GetBankAccountByAccountNumberAsync(Guid id)
    {
        var bankAccount = await _bankAccountRepository.GetByIdAsync(id);

        if (bankAccount == null)
        {
            return new ApiResponse(false, $"BankAccount with ID {id} not found", null);
        }

        var bankAccountDto = bankAccount.ToDto(); 

        return new ApiResponse(true, "BankAccount retrieved successfully", bankAccountDto);
    }

    public async Task<ApiResponse> GetBankAccountsByCustomerIdAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            return new ApiResponse(false, $"Customer with ID {id} not found", null);
        }

        var bankAccountsDto = customer.Accounts.Select(b => b.ToDto()).ToList();

        return new ApiResponse(true, $"BankAccounts for Customer Id: {id} retrieved successfully", bankAccountsDto);
    }
}