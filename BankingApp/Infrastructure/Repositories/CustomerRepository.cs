using System.Collections.Concurrent;
using System.Collections.Generic;
using BankingApp.Domain.Entities.Customers;
using BankingApp.Domain.Repositories;
using BankingApp.Domain.Services;
using BankingApp.Domain.Shared.OperationResults;
using BankingApp.Domain.ValueObjects.Emails;

namespace BankingApp.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository, IEmailUniquenessChecker
{   
    // we are not using real Data Base so for now we use thread safe collections
    private readonly ConcurrentDictionary<Guid, Customer> _customers = new();
    private readonly ConcurrentDictionary<string, bool> _emailRegistry = new();

    public OperationResult<Customer> Add(Customer customer)
    {
        if (_customers.TryAdd(customer.Id, customer))
        {   
             _emailRegistry.TryAdd(customer.EmailAddress.NormalizedValue, true);
             return OperationResult<Customer>.Success(customer, "Customer created successfully.");
        }

        return OperationResult<Customer>.Failure(null, "Failed to add customer.");
    }

    public IEnumerable<Customer> GetAll() => _customers.Values;

    public Customer? GetByEmail(Email email)
    {   
        if (!_emailRegistry.ContainsKey(email.NormalizedValue)) return null;

        return _customers.Values.FirstOrDefault(c =>
            c.EmailAddress.NormalizedValue == email.NormalizedValue);
    }

    public Customer? GetById(Guid id)
    {
        _customers.TryGetValue(id, out var customer);
        return customer;
    }

    public bool IsEmailUnique(Email email) => !_emailRegistry.ContainsKey(email.NormalizedValue);
}