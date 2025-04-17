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


}