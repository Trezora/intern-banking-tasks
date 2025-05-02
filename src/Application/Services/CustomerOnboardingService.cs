using Domain.Entities;
using Domain.Services;
using Shared.OperationResults;

namespace Application.Services;

public class CustomerOnboardingService : ICustomerOnBoardingService
{   
    private readonly ICustomerValidator _customerValidator;
    private readonly IOnboardingLogger _logger;
    private readonly decimal _defaultInitialBalance;

    public CustomerOnboardingService(
        ICustomerValidator customerValidator,
        IOnboardingLogger logger,
        decimal defaultInitialBalance = 0
    )
    {
        _customerValidator = customerValidator;
        _logger = logger;
        _defaultInitialBalance = defaultInitialBalance;
    }
    public OperationResult OnboardCustomer(Customer customer, decimal initialBalance = 0)
    {
        var validationResult = _customerValidator.ValidateCustomerForOnboarding(customer);
        if (!validationResult.Result)
        {
            _logger.LogOnboardingFailure(customer, validationResult.Message);
            return validationResult;
        }

        var effectiveInitialBalance = initialBalance > 0 ? initialBalance : _defaultInitialBalance;
        var account = customer.OpenNewAccount(effectiveInitialBalance);

        _logger.LogOnboardingSuccess(customer, account.AccountNumber, effectiveInitialBalance);
        return OperationResult.Succeeded($"Customer {customer.FullName} successfully onboarded with initial balance of {effectiveInitialBalance:C}");
    }
}