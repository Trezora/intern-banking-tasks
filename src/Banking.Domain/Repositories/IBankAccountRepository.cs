using Banking.Domain.Entities;
using Banking.Domain.ValueObjects;

namespace Banking.Domain.Repositories;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetByAccountNumberAsync(Guid accountNumber);
    Task AddAsync(BankAccount bankAccount);
    Task<List<BankAccount>> GetAccountsByCustomerIdAsync(Guid customerId);
    
}