using Banking.Domain.Entities;
using Banking.Domain.Services;
using Banking.Shared.OperationResults;

namespace Banking.Application.Services;

public class CustomerValidator : ICustomerValidator
{   
    private const int MinimumAgeRequirement = 18;
    public Result<Customer> ValidateCustomerForOnboarding(Customer customer)
    {
        if (customer.GetAge() < MinimumAgeRequirement)
            return Result<Customer>.FailureWith($"Customer must be at least {MinimumAgeRequirement} years old to open an account.");


        return Result<Customer>.Success(customer);
    }
}