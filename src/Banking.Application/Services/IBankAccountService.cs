using Banking.Application.DTOs.Responses;

namespace Banking.Application.Services;

public interface IBankAccountService
{
    Task<ApiResponse> GetBankAccountByAccountNumberAsync(Guid id);

    Task<ApiResponse> GetBankAccountsByCustomerIdAsync(Guid id);

}