using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;
using Banking.Shared.OperationResults;

namespace Banking.Domain.Services;

public interface ICustomerOnBoardingService
{
    Result<Customer> OnboardCustomer(Customer customer, Money initialBalance);
}