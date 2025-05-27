using Banking.Application.DTOs;
using Banking.Domain.Shared;

namespace Banking.Application.Services;

public interface ICustomerService
{
    Task<Result<CustomerDto>> TryGetCustomerByIdAsync(Guid id);

    Task<Result<IEnumerable<CustomerDto>>> TryGetAllCustomerAsync();
}