using System.Reflection.Metadata;
using Banking.Application.Services;
using Banking.Domain.Entities;
using Banking.Domain.Factories;
using Banking.Domain.Services;
using Moq;
using Banking.Shared.OperationResults;
using Banking.Domain.ValueObjects;

namespace Application.Tests;

public class CustomerOnboardingTests
{
    private readonly Customer _validCustomerWithValidAge;
    private readonly Customer _validCustomerWithInvalidAge;
    private readonly CustomerFactory _customerFactory;
    private readonly Mock<IOnboardingLogger> _mockLogger;
    private readonly Mock<ICustomerValidator> _mockValidator;
    private const decimal _defaultInitialBalance = 100.00m;

    public CustomerOnboardingTests()
    {
        _customerFactory = new CustomerFactory();
        _validCustomerWithValidAge = _customerFactory.CreateCustomer(
            "Beka Buliskeria", 
            "abcd@gmail.com", 
            new(2003, 8, 27)
        );

        _validCustomerWithInvalidAge = _customerFactory.CreateCustomer(
            "Beka Buliskeria", 
            "abcde@gmail.com", 
            new(2010, 8, 27)
        );

        _mockLogger = new Mock<IOnboardingLogger>();
        _mockValidator = new Mock<ICustomerValidator>();

    }

    [Fact]
    public void OnboardCustomer_WithValidCustomer_WithValidAge_ShouldSucceed()
    {   
        // Arrange
        var onboardedCustomerService = new CustomerOnboardingService(
            _mockValidator.Object,
            _mockLogger.Object,
            new Money(_defaultInitialBalance)
        );

        _mockValidator
            .Setup(v => v.ValidateCustomerForOnboarding(_validCustomerWithValidAge))
            .Returns(OperationResult.Succeeded("Customer is eligible for onboarding."));

        // Act
        var result = onboardedCustomerService.OnboardCustomer(_validCustomerWithValidAge, new Money(0));

        // Assert
        Assert.True(result.Result);

        _mockLogger.Verify(
            l => l.LogOnboardingSuccess(
                _validCustomerWithValidAge, 
                It.IsAny<Guid>(), 
                new Money(_defaultInitialBalance)
            ),
            Times.Once
        );

        _mockLogger.Verify(
            l => l.LogOnboardingFailure(
                It.IsAny<Customer>(), 
                It.IsAny<string>()
            ),
            Times.Never
        );
    }

    [Fact]
    public void OnboardCustomer_WithValidCustomer_WithInvalidAge_ShouldFail()
    {
        // Arrange
        var failureMessage = "Customer must be at least 18 years old to open an account.";
        
        var onboardingService = new CustomerOnboardingService(
            _mockValidator.Object,
            _mockLogger.Object,
            new Money(_defaultInitialBalance)
        );

        _mockValidator
            .Setup(v => v.ValidateCustomerForOnboarding(_validCustomerWithInvalidAge))
            .Returns(OperationResult.Failed(failureMessage));

        // Act
        var result = onboardingService.OnboardCustomer(_validCustomerWithInvalidAge, new Money(0.00m));

        // Assert
        Assert.False(result.Result);
        Assert.Equal(failureMessage, result.Message);

        _mockLogger.Verify(
            l => l.LogOnboardingFailure(
                _validCustomerWithInvalidAge, 
                failureMessage
            ),
            Times.Once
        );

        _mockLogger.Verify(
            l => l.LogOnboardingSuccess(
                It.IsAny<Customer>(), 
                It.IsAny<Guid>(), 
                It.IsAny<Money>()
            ),
            Times.Never
        );
    }
}