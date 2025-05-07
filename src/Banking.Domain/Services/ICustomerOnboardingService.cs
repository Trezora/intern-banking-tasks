using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;
using Banking.Shared.OperationResults;

namespace Banking.Domain.Services;

public interface ICustomerOnBoardingService
{
    OperationResult OnboardCustomer(Customer customer, Money initialBalance);
}