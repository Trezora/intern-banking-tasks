using Banking.Domain.Entities;

namespace Banking.Domain.Repositories;

public interface ICustomerRespository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<List<Customer>> GetAllCustomerAsync();
    Task AddAsync(Customer customer);
}