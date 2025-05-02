using Domain.Entities;
using Shared.OperationResults;

namespace Domain.Services;

public interface ICustomerOnBoardingService
{
    OperationResult OnboardCustomer(Customer customer, decimal initialBalance = 0);
}