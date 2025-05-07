using Banking.Domain.Entities;
using Banking.Domain.Exceptions;
using Banking.Domain.Factories;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Tests;

public class CustomerTests 
{

    [Fact]
    public void Should_create_valid_customer()
    {   
        // Arrange
        var customerFactory = new CustomerFactory();

        // Act
        var newCustomer = customerFactory.CreateCustomer(
            new Name("Beka Buliskeria"), 
            new Email("abcd@gmail.com"), 
            new(2003, 8, 27)
        );

        // Assert
        Assert.NotNull(newCustomer);
        Assert.Equal("Beka Buliskeria", newCustomer.FullName);
        Assert.Equal("abcd@gmail.com", newCustomer.EmailAddress);
        Assert.Equal(new DateTime(2003, 8, 27), newCustomer.DateOfBirth);
    }

    [Fact]
    public void Should_throw_EmptyCustomerNameException_during_customer_creation_with_no_name()
    {
        // Arrange
        var customerFactory = new CustomerFactory();

        // Act & Assert
        var exception = Assert.Throws<EmptyCustomerNameException>(() =>
            customerFactory.CreateCustomer(            
                new Name(""),
                new Email("abcd@gmail.com"),
                new(2002, 2, 5)));
    }

    [Fact]
    public void Should_throw_InvalidEmailFormatException_during_customer_creation_with_invalid_email()
    {
        // Arrange
        var customerFactory = new CustomerFactory();

        // Act & Assert
        var exception = Assert.Throws<InvalidEmailFormatException>(() =>
            customerFactory.CreateCustomer(            
                new Name("Beka Buliskeria"),
                new Email("abcdgmail.com"),
                new(2002, 2, 5)));
    }

    [Fact]
    public void Should_throw_EmailAlreadyExistException_during_customer_creation_with_same_email()
    {
        // Arrange
        var customerFactory = new CustomerFactory();

        // Act
        var newCustomer_1 = customerFactory.CreateCustomer(
            new Name("Beka Buliskeria"), 
            new Email("abcd@gmail.com"), 
            new(2003, 8, 27)
        );

        // Act & Assert
        Assert.NotNull(newCustomer_1);
        Assert.Equal("Beka Buliskeria", newCustomer_1.FullName);
        Assert.Equal("abcd@gmail.com", newCustomer_1.EmailAddress);
        Assert.Equal(new DateTime(2003, 8, 27), newCustomer_1.DateOfBirth);

        var exception = Assert.Throws<EmailAlreadyExistException>(() =>
            customerFactory.CreateCustomer(            
                new Name("John Snow"),
                new Email("abcd@gmail.com"),
                new(2002, 2, 5)));
    }

    [Fact]
    public void Should_return_customer_summary()
    {
        // Arrange
        var customerFactory = new CustomerFactory();

        // Act
        var newCustomer = customerFactory.CreateCustomer(
            new Name("Beka Buliskeria"), 
            new Email("abcd@gmail.com"), 
            new(2003, 8, 27)
        );

        var summary = "Customer summary:\n" +
                       "  - Full name: Beka Buliskeria\n" +
                       "  - Email: abcd@gmail.com\n" +
                      $"  - DateTime: {new DateTime(2003, 8, 27):yyyy-MM-dd}";

        // Assert
        Assert.NotNull(newCustomer);
        Assert.Equal(summary, newCustomer.GetCustomerSummary());
    }

    [Fact]
    public void Should_return_age_of_customer()
    {
        // Arrange
        var customerFactory = new CustomerFactory();

        // Act
        var newCustomer_1 = customerFactory.CreateCustomer(
            new Name("Beka Buliskeria"), 
            new Email("abcd@gmail.com"), 
            new(2003, 8, 27)
        );

        var newCustomer_2 = customerFactory.CreateCustomer(
            new Name("Beka Buliskeria"), 
            new Email("abc@gmail.com"), 
            new(2003, 2, 27)
        );

        var age_1 = newCustomer_1.GetAge();
        var age_2 = newCustomer_2.GetAge();

        // Assert
        Assert.Equal(21, age_1);
        Assert.Equal(22, age_2);
    }

    [Fact]
    public void Should_create_several_bank_accounts_and_return_list_of_accounts()
    {
        // Arrange 
        var customerFactory1 = new CustomerFactory();

        // Act
        var customer = customerFactory1.CreateCustomer(
            new Name("Beka Buliskeria"), 
            new Email("abcd@gmail.com"), 
            new(2003, 8, 27)
        );

        // Act
        var noAccountList = customer.ListAccounts().ToList();

        // Assert
        Assert.Single(noAccountList);
        Assert.Equal("Customer has no bank accounts.", noAccountList.First());

        // Act
        var firstBankAccount = customer.OpenNewAccount(new Money(0.00m));
        var oneAccountList = customer.ListAccounts().ToList();

        // Assert
        Assert.Single(oneAccountList);
        Assert.Equal(firstBankAccount.PrintAccountSummary(), oneAccountList.First());
        Assert.Equal(0.00m, firstBankAccount.GetBalance().Value);

        // Act
        var secondBankAccont = customer.OpenNewAccount(100.00m);
        var thirdBankAccount = customer.OpenNewAccount(1000.00m);
        var fourthBankAccount = customer.OpenNewAccount(500.00m);

        var severalAccountList = customer.ListAccounts().ToList();

        // Assert
        Assert.Equal(4, severalAccountList.Count);
        Assert.Equal(secondBankAccont.PrintAccountSummary(), severalAccountList[1]);
        Assert.Equal(thirdBankAccount.PrintAccountSummary(), severalAccountList[2]);
        Assert.Equal(fourthBankAccount.PrintAccountSummary(), severalAccountList[3]);
        Assert.Equal(100.00m, secondBankAccont.GetBalance().Value);
        Assert.Equal(1000.00m, thirdBankAccount.GetBalance().Value);
        Assert.Equal(500.00m, fourthBankAccount.GetBalance().Value);
    }

}