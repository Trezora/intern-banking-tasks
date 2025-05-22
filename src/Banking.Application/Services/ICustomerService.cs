using Banking.Application.DTOs;
using Banking.Application.DTOs.Requests;
using Banking.Application.DTOs.Responses;
using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;
using Banking.Shared.OperationResults;

namespace Banking.Application.Services;

public interface ICustomerService
{
    Task<Result<CustomerDto>> TryGetCustomerByIdAsync(Guid id);

    Task<Result<IEnumerable<CustomerDto>>> TryGetAllCustomerAsync();
}