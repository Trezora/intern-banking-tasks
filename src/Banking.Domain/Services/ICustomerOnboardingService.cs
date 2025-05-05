using Banking.Domain.Entities;
using Banking.Shared.OperationResults;

namespace Banking.Domain.Services;

public interface ICustomerOnBoardingService
{
    OperationResult OnboardCustomer(Customer customer, decimal initialBalance = 0);
}