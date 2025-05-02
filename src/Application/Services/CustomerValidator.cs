using Domain.Entities;
using Domain.Services;
using Shared.OperationResults;

namespace Application.Services;

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