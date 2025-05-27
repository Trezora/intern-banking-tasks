using Banking.Application.DTOs;
using Banking.Domain.Shared;

namespace Banking.Application.Services;

public interface IBankAccountService
{
    Task<Result<BankAccountDto>> TryGetBankAccountByAccountNumberAsync(Guid id);

    Task<Result<IEnumerable<BankAccountDto>>> TryGetBankAccountsByCustomerIdAsync(Guid id);

}