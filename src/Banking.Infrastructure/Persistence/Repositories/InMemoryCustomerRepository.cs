using Banking.Domain.Entities;
using Banking.Domain.Repositories;

namespace Banking.Infrastructure.Persistence.Repositories;

public class InMemoryCustomerRepository : ICustomerRespository
{   
    private readonly Dictionary<Guid, Customer> _customers = [];
    public Task AddAsync(Customer customer)
    {
        _customers[customer.CustomerId] = customer;
        return Task.CompletedTask;
    }

    public Task<List<Customer>> GetAllCustomerAsync()
    {
        var customers = _customers.Values.ToList();
        return Task.FromResult(customers);
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        _customers.TryGetValue(id, out var customer);
        return Task.FromResult(customer);
    }
}
