using Banking.Application.DTOs;
using Banking.Application.DTOs.Responses;
using Banking.Shared.OperationResults;

namespace Banking.Application.Services;

public interface IBankAccountService
{
    Task<Result<BankAccountDto>> TryGetBankAccountByAccountNumberAsync(Guid id);

    Task<Result<IEnumerable<BankAccountDto>>> TryGetBankAccountsByCustomerIdAsync(Guid id);

}