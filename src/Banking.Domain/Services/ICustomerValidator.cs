using Banking.Domain.Entities;
using Banking.Shared.OperationResults;

namespace Banking.Domain.Services;

public interface ICustomerValidator
{
    OperationResult ValidateCustomerForOnboarding(Customer customer);
}