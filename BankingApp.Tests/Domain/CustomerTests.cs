using BankingApp.Domain.Entities.Customers;
using BankingApp.Application.Services;
using BankingApp.Domain.Repositories;
using BankingApp.Domain.Services;
using BankingApp.Domain.Shared.OperationResults;
using BankingApp.Domain.ValueObjects.Emails;
using Moq;

namespace BankingApp.Domain.CustomerTest;

public class CustomerTests
{   
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly Mock<IEmailUniquenessChecker> _mockEmailChecker;
    private readonly CustomerService _customerService;

    public CustomerTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockEmailChecker = new Mock<IEmailUniquenessChecker>();
        _mockEmailChecker.Setup(x => x.IsEmailUnique(It.IsAny<Email>())).Returns(true);
        _mockCustomerRepository.Setup(x => x.Add(It.IsAny<Customer>())).Returns((Customer c) => 
            OperationResult<Customer>.Success(c, "Customer added successfully"));
        
        _customerService = new CustomerService(_mockCustomerRepository.Object, _mockEmailChecker.Object);
    }

    [Fact]
    public void CreateCustomerWithValidData_ShouldCreateCustomer()
    {
        // Arrange
        var validFullName = "Beka Buliskeria";
        var validEmail = "abcd@gmail.com";
        var dateOfBirth = new DateTime(2003, 8, 27);

        // Act
        var result = _customerService.CreateCustomer(validFullName, validEmail, dateOfBirth);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(validFullName, result.Data!.FullName.ToString());
        Assert.Equal(validEmail, result.Data.EmailAddress.ToString());
        Assert.Equal(dateOfBirth, result.Data.DateOfBirth);
    }

    [Fact]
    public void CreateCustomerWithInvalidNameFormat_ShouldNotCreateCustomer()
    {
        // Arrange
        var invalidFullName = "abcdefgh";
        var validEmail = "abcd@gmail.com";
        var dateOfBirth = new DateTime(2003, 8, 27);

        // Act
        var result = _customerService.CreateCustomer(invalidFullName, validEmail, dateOfBirth);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Contains("Full name must be in the format 'First Last'", result.Message);
    }

    [Fact]
    public void CreateCustomerWithInvalidNameLength_ShouldNotCreateCustomer()
    {
        // Arrange
        var invalidFullName = "abcdefghij klmnopqrstuvwxyz";
        var validEmail = "abcd@gmail.com";
        var dateOfBirth = new DateTime(2003, 8, 27);

        // Act
        var result = _customerService.CreateCustomer(invalidFullName, validEmail, dateOfBirth);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Contains("Full name must not exceed", result.Message);
    }
    
    [Fact]
    public void CreateCustomerWithInvalidEmailFormat_ShouldNotCreateCustomer()
    {   
        // Arrange
        var validFullName = "Beka Buliskeria";
        var invalidEmail = "askdfj2kjasdkj.asdf";
        var dateOfBirth = new DateTime(2003, 8, 27);

        // Act
        var result = _customerService.CreateCustomer(validFullName, invalidEmail, dateOfBirth);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Contains("Invalid email format", result.Message);
    }

    [Fact]
    public void CreateCustomerAndGetAgeOfCustomerBeforeLastBirthday()
    {
        // Arrange
        var validFullName = "Beka Bulikseria";
        var validEmail = "abcd@gmail.com";
        var dateOfBirth = new DateTime(2003, 8, 27);
        var result = _customerService.CreateCustomer(validFullName, validEmail, dateOfBirth);

        // Act
        var customer = result.Data;
        var customerSummary = customer!.GetCustomerSummary();
        var age = customer.GetAge();

        // Assert
        Assert.Equal(21, age);
    }

    [Fact]  
    public void CreateCustomerAndGetAgeOfCustomerAfterLastBirthday()
    {
        // Arrange
        var validFullName = "Beka Bulikseria";
        var validEmail = "abcd@gmail.com";
        var dateOfBirth = new DateTime(2003, 3, 27);
        var result = _customerService.CreateCustomer(validFullName, validEmail, dateOfBirth);

        // Act
        var customer = result.Data;
        var customerSummary = customer!.GetCustomerSummary();
        var age = customer.GetAge();

        // Assert
        Assert.Equal(22, age);
    }

    [Fact]
    public void CreateCustomerAndTestGetCustomerSummary()
    {   
        // Arrange
        var validFullName = "Beka Bulikseria";
        var validEmail = "abcd@gmail.com";
        var dateOfBirth = new DateTime(2003, 8, 27);
        var result = _customerService.CreateCustomer(validFullName, validEmail, dateOfBirth);
        var customer = result.Data;

        // Act
        var customerSummary = customer!.GetCustomerSummary();

        var validResult = "Customer summary:\n" +
                          $"  - Full name: {customer.FullName}\n" +
                          $"  - Email: {customer.EmailAddress}\n" +
                          $"  - DateTime: {customer.DateOfBirth.ToString("yyyy-MM-dd")}";

        // Assert
        Assert.NotNull(customer);
        Assert.Equal(validResult, customerSummary);
        Assert.Contains(customer.FullName.ToString(), customerSummary);
        Assert.Contains(customer.EmailAddress.ToString(), customerSummary);
        Assert.Contains(customer.DateOfBirth.ToString("yyyy-MM-dd"), customerSummary);
    }

    [Fact]
    public void CreateCustomer_EmailAlreadyInUse_ShouldReturnFailure()
    {
        // Arrange
        var validFullName = "Beka Buliskeria";
        var validEmail = "existing@gmail.com";
        var dateOfBirth = new DateTime(2003, 8, 27);
        
        // Setup email checker to return false (email not unique)
        _mockEmailChecker.Setup(x => x.IsEmailUnique(It.IsAny<Email>())).Returns(false);

        // Act
        var result = _customerService.CreateCustomer(validFullName, validEmail, dateOfBirth);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Equal("Email address is already in use.", result.Message);
    }
}