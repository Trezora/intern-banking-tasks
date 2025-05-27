using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Repositories;

public interface ICustomerRespository
{
    Task<Customer?> GetByCustomerIdAsync(Guid customerId);
    Task<List<Customer>> GetAllCustomerAsync();
    Task AddAsync(Customer customer);
    Task<bool> CustomerExistsWithSameEmailAsync(Email email);
}