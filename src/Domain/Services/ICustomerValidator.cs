using Domain.Entities;
using Shared.OperationResults;

namespace Domain.Services;

public interface ICustomerValidator
{
    OperationResult ValidateCustomerForOnboarding(Customer customer);
}