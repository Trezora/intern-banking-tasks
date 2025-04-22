using BankingApp.Domain.Entities.Customers;
using BankingApp.Domain.Repositories;
using BankingApp.Domain.Services;
using BankingApp.Domain.Shared.OperationResults;
using BankingApp.Domain.ValueObjects.Emails;

namespace BankingApp.Application.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmailUniquenessChecker _emailUniquenessChecker;

    public CustomerService(
        ICustomerRepository customerRepository,
        IEmailUniquenessChecker emailUniquenessChecker)
    {
        _customerRepository = customerRepository;
        _emailUniquenessChecker = emailUniquenessChecker;
    }

    public OperationResult<Customer> CreateCustomer(string fullName, string emailAddress, DateTime dateOfBirth)
    {
        try
        {
            var email = Email.Create(emailAddress);

            if (!_emailUniquenessChecker.IsEmailUnique(email))
                return OperationResult<Customer>.Failure(null, "Email address is already in use.");
            
            var customer = new Customer(fullName, emailAddress, dateOfBirth);

            return _customerRepository.Add(customer);
        }
        catch (ArgumentException ex)
        {
            return OperationResult<Customer>.Failure(null, ex.Message);
        }
    }

    public Customer? GetCustomerByEmail(string emailAddress)
    {
        try
        {
            var email = Email.Create(emailAddress);
            return _customerRepository.GetByEmail(email);
        }
        catch
        {
            return null;
        }
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _customerRepository.GetAll();
    }
}