using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.ValueObjects;

namespace Banking.Application.Services;

public interface ICustomerService
{
    Task<ApiResponse> CreateCustomerAsync(CreateCustomerRequest request);

    Task<ApiResponse> OpenNewBankAccountAsync(CreateBankAccountRequest request);

    Task<ApiResponse> GetCustomerByIdAsync(Guid id);

    Task<ApiResponse> GetAllCustomerAsync();
}