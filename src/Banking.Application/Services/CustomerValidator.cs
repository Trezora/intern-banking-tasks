using Banking.Domain.Entities;
using Banking.Domain.Services;
using Banking.Shared.OperationResults;

namespace Banking.Application.Services;

public class CustomerValidator : ICustomerValidator
{   
    private const int MinimumAgeRequirement = 18;
    public OperationResult ValidateCustomerForOnboarding(Customer customer)
    {
        if (customer.GetAge() < MinimumAgeRequirement)
            return OperationResult.Failed($"Customer must be at least {MinimumAgeRequirement} years old to open an account.");


        return OperationResult.Succeeded("Customer is eligible for onboarding.");
    }
}