using BankingApp.Domain.Entities.Customers;

namespace BankingApp.Domain.CustomerTest;

public class CustomerTests
{   

    [Fact]
    public void CreateCustomerWithValidData_ShouldCreateCustomer()
    {
        // Arrange
        var validFullName = "Beka Buliskeria";
        var validEmail = "abcd@gmail.com";
        var DateOfBirth = new DateTime(2003, 8, 27);

        // Act
        var validCustomer = new Customer(validFullName, validEmail, DateOfBirth);

        // Assert
        Assert.NotNull(validCustomer);
        Assert.Equal(validFullName, validCustomer.FullName.ToString());
        Assert.Equal(validEmail, validCustomer.EmailAddress.ToString());
        Assert.Equal(DateOfBirth, validCustomer.DateOfBirth);
    }

    [Fact]
    public void CreateCustomerWitInvalidNameFormat_ShouldNotCreateCustomer()
    {
        // Arrange
        var invalidFullName = "abcdefgh";
        var validEmail = "abcd@gmail.com";
        var DateOfBirth = new DateTime(2003, 8, 27);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new Customer(invalidFullName, validEmail, DateOfBirth));

        // Assert
        Assert.Equal("Full name must be in the format 'First Last'.", exception.Message);
    }

    [Fact]
    public void CreateCustomerWitInvalidNameLength_ShouldNotCreateCustomer()
    {
        // Arrange
        var invalidFullName = "abcdefghij klmnopqrstuvwxyz";
        var validEmail = "abcd@gmail.com";
        var DateOfBirth = new DateTime(2003, 8, 27);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            new Customer(invalidFullName, validEmail, DateOfBirth));

    }
    
    [Fact]
    public void CreateCustomerWithInvalidEmailFormat_ShouldNotCreateCustomer()
    {   
        // Arrange
        var validFullName = "Beka Buliskeria";
        var invalidEmail = "askdfj2kjasdkj.asdf";
        var DateOfBirth = new DateTime(2003, 8, 27);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(validFullName, invalidEmail, DateOfBirth));
    }

    [Fact]
    public void CreateCustomerAndGetAgeOfCustomerBeforeLastBirthday()
    {
        // Arrange
        var customer = new Customer(
            fullName: "Beka Bulikseria",
            emailAddress: "abcd@gmail.com",
            dateOfBirth: new DateTime(2003, 8, 27)
        );

        // Act
        var customerSummary = customer.GetCustomerSummary();
        var age = customer.GetAge();

        // Assert
        Assert.Equal(21, age);
    }

    [Fact]  
    public void CreateCustomerAndGetAgeOfCustomerAfterLastBirthday()
    {
        // Arrange
        var customer = new Customer(
            fullName: "Beka Bulikseria",
            emailAddress: "abcd@gmail.com",
            dateOfBirth: new DateTime(2003, 3, 27)
        );

        // Act
        var customerSummary = customer.GetCustomerSummary();
        var age = customer.GetAge();

        // Assert
        Assert.Equal(22, age);
    }

    [Fact]
    public void CreateCustomerAndTestGetCustomerSummary()
    {   
        // Arrange
        var customer = new Customer(
            fullName: "Beka Bulikseria",
            emailAddress: "abcd@gmail.com",
            dateOfBirth: new DateTime(2003, 8, 27)
        );

        // Act
        var customerSummary = customer.GetCustomerSummary();

        var validResult = "Customer summary:\n" +
                          $"  - Full name: {customer.FullName.ToString()}\n" +
                          $"  - Email: {customer.EmailAddress.ToString()}\n" +
                          $"  - DateTime: {customer.DateOfBirth.ToString("yyyy-MM-dd")}";

        // Assert
        Assert.NotNull(customer);

        Assert.Equal(customerSummary, validResult);
        Assert.Contains(customer.FullName.ToString(), customerSummary);
        Assert.Contains(customer.EmailAddress.ToString(), customerSummary);
        Assert.Contains(customer.DateOfBirth.ToString("yyyy-MM-dd"), customerSummary);
    }

}