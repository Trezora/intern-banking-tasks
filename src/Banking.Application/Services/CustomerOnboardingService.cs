using Banking.Domain.Entities;
using Banking.Domain.Services;
using Banking.Domain.ValueObjects;
using Banking.Shared.OperationResults;

namespace Banking.Application.Services;

public class CustomerOnboardingService : ICustomerOnBoardingService
{   
    private readonly ICustomerValidator _customerValidator;
    private readonly IOnboardingLogger _logger;
    private readonly Money _defaultInitialBalance;

    public CustomerOnboardingService(
        ICustomerValidator customerValidator,
        IOnboardingLogger logger,
        Money defaultInitialBalance
    )
    {
        _customerValidator = customerValidator;
        _logger = logger;
        _defaultInitialBalance = defaultInitialBalance;
    }
    public OperationResult OnboardCustomer(Customer customer, Money initialBalance)
    {
        var validationResult = _customerValidator.ValidateCustomerForOnboarding(customer);
        if (!validationResult.Result)
        {
            _logger.LogOnboardingFailure(customer, validationResult.Message);
            return validationResult;
        }

        var effectiveInitialBalance = initialBalance.Value > 0 ? initialBalance.Value : _defaultInitialBalance.Value;
        var account = customer.OpenNewAccount(effectiveInitialBalance);

        _logger.LogOnboardingSuccess(customer, account.AccountNumber, effectiveInitialBalance);
        return OperationResult.Succeeded($"Customer {customer.FullName} successfully onboarded with initial balance of {effectiveInitialBalance:C}");
    }
}