using BankingApp.Domain.Entities.Customers;
using BankingApp.Domain.Shared.OperationResults;
using BankingApp.Domain.ValueObjects.Emails;

namespace BankingApp.Domain.Repositories;

public interface ICustomerRepository
{
    OperationResult<Customer> Add(Customer customer);
    bool IsEmailUnique(Email email);
    Customer? GetByEmail(Email email);
    Customer? GetById(Guid id);
    IEnumerable<Customer> GetAll();
}