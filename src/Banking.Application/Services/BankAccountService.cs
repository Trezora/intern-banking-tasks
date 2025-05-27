using Banking.Application.DTOs;
using Banking.Application.Mappings;
using Banking.Domain.Repositories;
using Banking.Domain.Shared;

namespace Banking.Application.Services;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountService(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }
    
    public async Task<Result<BankAccountDto>> TryGetBankAccountByAccountNumberAsync(Guid id)
    {
        var bankAccount = await _bankAccountRepository.GetByAccountNumberAsync(id);

        if (bankAccount == null)
            return Result<BankAccountDto>.FailureWith("Bank account", $"Bank account with account number {id} was not found.");
    
        return Result<BankAccountDto>.Success(bankAccount.ToDto());
    }

    public async Task<Result<IEnumerable<BankAccountDto>>> TryGetBankAccountsByCustomerIdAsync(Guid id)
    {
        var bankAccounts = await _bankAccountRepository.GetAccountsByCustomerIdAsync(id);

        if (bankAccounts == null || !bankAccounts.Any())
            return Result<IEnumerable<BankAccountDto>>.FailureWith("Bank account", "No bank Accounts found.");

        var BankAccountDto = bankAccounts.Select(c => c.ToDto());
        
        return Result<IEnumerable<BankAccountDto>>.Success(BankAccountDto);
    }
}